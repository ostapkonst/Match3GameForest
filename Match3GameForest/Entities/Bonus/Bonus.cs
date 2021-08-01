using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Entities
{
    public class Bonus : Sprite, IBonus
    {
        public Bonus(ISpriteBatch spriteBatch, ITexture2D spriteStrip)
        : base(spriteBatch, spriteStrip)
        {
        }

        private void AssignEvents(IList<IEnemy> enemies)
        {
            foreach (var enemy in enemies) {
                var bonus = (Bonus)Clone();
                enemy.AfterUpdate += bonus.Update;
                enemy.AfterDraw += bonus.Draw;
                enemy.OnPosition += (value) => bonus.Position = value;
                enemy.OnScale += (value) => bonus.Scale = value;
                enemy.OnLightning += (value) => bonus.Lightning = value;
            }
        }

        public FieldSeries Build(FieldSeries series)
        {
            var list = new List<IEnemy>();

            foreach (var el in series.Series) {
                if (el.Count != 4) continue;
                var maxTimeEnemy = el[0];
                foreach (var enemy in el) {
                    if (enemy.TouchedTime > maxTimeEnemy.TouchedTime) {
                        maxTimeEnemy = enemy;
                    }
                }
                var clone = (IEnemy)maxTimeEnemy.Clone();
                list.Add(clone);
            }

            AssignEvents(list);

            return new FieldSeries(list);
        }
    }
}
