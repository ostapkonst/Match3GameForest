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
        public IEnemy Carrier { get; protected set; }

        public IList<MatrixPos> Series { get; protected set; }
        public bool IsActivate { get; set; }

        public Bonus(ISpriteBatch spriteBatch, ITexture2D spriteStrip)
        : base(spriteBatch, spriteStrip)
        {
        }

        private IBonus AssignEvents(IEnemy enemy)
        {
            var bonus = (Bonus)Clone();

            enemy.AfterUpdate += bonus.Update;
            enemy.AfterDraw += bonus.Draw;
            enemy.OnPosition += (value) => bonus.Position = value;
            enemy.OnScale += (value) => bonus.Scale = value;
            enemy.OnLightning += (value) => bonus.Lightning = value;
            enemy.OnDestroy += () =>
            {
                bonus.IsActivate = true;
            };

            bonus.Carrier = enemy;
            bonus.IsActivate = false;

            return bonus;
        }

        public IList<IBonus> Build(FieldSeries series)
        {
            var bonuses = new List<IBonus>();

            foreach (var el in series.Series) {
                if (el.Count < 4) continue;
                var maxTimeEnemy = el[0];
                foreach (var enemy in el) {
                    if (enemy.TouchedTime > maxTimeEnemy.TouchedTime) {
                        maxTimeEnemy = enemy;
                    }
                }
                var clone = (IEnemy)maxTimeEnemy.Clone();
                bonuses.Add(AssignEvents(clone));
            }

            return bonuses;
        }
    }
}
