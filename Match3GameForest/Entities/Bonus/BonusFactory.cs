using System.Collections.Generic;
using System.Linq;
using Match3GameForest.Config;
using Match3GameForest.Core;

namespace Match3GameForest.Entities
{
    public class BonusFactory : IBonusFactory, IRegistering
    {
        private readonly IBonus[] _bonuses;

        private readonly IList<IBonus> _createdBonuses;

        public float Scale { get; set; } = 0.5f;

        public BonusFactory(IContentManager contentManager, ISpriteBatch spriteBatch)
        {
            ITexture2D texture1 = contentManager.LoadTexture("BombBonus"),
                texture2 = contentManager.LoadTexture("LineBonus"),
                texture3 = contentManager.LoadTexture("Destroyer");

            _bonuses = new IBonus[] {
                new BombBonus(spriteBatch, texture1) { Scale = Scale},
                new LineBonus(spriteBatch, texture2, texture3) {
                    Scale = Scale, DestroyerScale = Scale
                }
            };

            _createdBonuses = new List<IBonus>();
        }

        public FieldSeries Build(FieldSeries gameField)
        {
            var enemies = new List<IEnemy>();

            foreach (var bonus in _bonuses) {
                var tmp = bonus.Build(gameField);
                foreach (var newBonus in tmp) {
                    _createdBonuses.Add(newBonus);
                    enemies.Add(newBonus.Carrier);
                }
            }

            return new FieldSeries(enemies);
        }

        public bool IsActivate { get => _createdBonuses.Any(x => x.IsActive); }

        public List<IBonus> GetActiveBonuses()
        {
            return _createdBonuses.Where(x => x.IsActive).ToList();
        }

        public void Clear()
        {
            _createdBonuses.Clear();
        }
    }
}
