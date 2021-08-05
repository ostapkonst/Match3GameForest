using System.Collections.Generic;
using Match3GameForest.Core;

namespace Match3GameForest.Entities
{
    public class BombBonus : Bonus
    {
        public BombBonus(ISpriteBatch spriteBatch, ITexture2D spriteStrip)
        : base(spriteBatch, spriteStrip)
        {
        }

        public override IList<IBonus> Build(FieldSeries series)
        {
            var bonuses = new List<IBonus>();

            foreach (var el in series.Series) {
                if (el.Count < 5) continue;
                var maxTimeEnemy = el[0];
                foreach (var enemy in el) {
                    if (enemy.TouchedTime > maxTimeEnemy.TouchedTime) {
                        maxTimeEnemy = enemy;
                    }
                }
                bonuses.Add(AssignEvents(maxTimeEnemy));
            }

            return bonuses;
        }
    }
}
