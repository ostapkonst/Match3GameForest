using System;
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

        public float Scale = 0.5f;

        public BonusFactory(IContentManager contentManager, ISpriteBatch spriteBatch)
        {
            _bonuses = new IBonus[] {
                new Bonus(spriteBatch, contentManager.LoadTexture("BombBonus")) { Scale = Scale},
            };
        }

        public FieldSeries Build(FieldSeries gameField)
        {
            var enemiesWithBonus = new List<IEnemy>();

            foreach (var bonus in _bonuses) {
                enemiesWithBonus.AddRange(bonus.Build(gameField).Line);
            }

            return new FieldSeries(enemiesWithBonus);
        }
    }
}
