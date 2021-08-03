using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3GameForest.Config;
using Match3GameForest.Core;

namespace Match3GameForest.Entities
{
    public class BonusFactory : IBonusFactory, IRegistering
    {
        private readonly IBonus[] _bonuses;

        private ConcurrentBag<IBonus> _createdBonuses;

        public float Scale { get; set; } = 0.5f;

        public BonusFactory(IContentManager contentManager, ISpriteBatch spriteBatch)
        {
            _bonuses = new IBonus[] {
                new Bonus(spriteBatch, contentManager.LoadTexture("BombBonus")) { Scale = Scale},
            };

            _createdBonuses = new ConcurrentBag<IBonus>();
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

        public bool IsActivate { get => _createdBonuses.Any(x => x.Status == BonusStatus.Activated); }

        public List<IBonus> GetActiveBonuses()
        {
            return _createdBonuses.Where(x => x.Status == BonusStatus.Activated).ToList();
        }

        public void Clear()
        {
            _createdBonuses.Clear();
        }
    }
}
