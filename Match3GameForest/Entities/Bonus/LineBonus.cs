using System;
using System.Collections.Generic;
using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Entities
{
    public class LineBonus : Bonus
    {
        private readonly ISpriteBatch _spriteBatch;
        private readonly ITexture2D _destroyer;

        public float DestroyerScale { get; set; }

        private readonly Random _random;

        public LineBonus(ISpriteBatch spriteBatch, ITexture2D spriteStrip, ITexture2D destroyer)
        : base(spriteBatch, spriteStrip)
        {
            _spriteBatch = spriteBatch;
            _destroyer = destroyer;
            DestroyerScale = 1f;
            _random = new Random();
        }

        public Destroyer GetDestroyer(bool first = true)
        {
            var destroyer = new Destroyer(_spriteBatch, _destroyer);

            AfterUpdate += (value) => destroyer.Update(value);
            AfterDraw += (value) => destroyer.Draw(value);
            destroyer.Position = Position;
            destroyer.Scale = DestroyerScale;
            destroyer.Rotation = Rotation;
            destroyer.Direction = Rotation == 0f ? new Vector2(1, 0) : new Vector2(0, 1);

            if (!first) {
                destroyer.Direction *= -1;
                destroyer.Rotation += 1;
            }

            return destroyer;
        }

        public override IList<IBonus> Build(FieldSeries series)
        {
            var bonuses = new List<IBonus>();
            var hash = new HashSet<IEnemy>();

            foreach (var el in series.Series) {
                var maxTimeEnemy = el[0];

                foreach (var enemy in el) {
                    if (enemy.TouchedTime > maxTimeEnemy.TouchedTime) {
                        maxTimeEnemy = enemy;
                    }

                    if (hash.Contains(enemy)) { // является пересечением
                        if (_random.NextBool()) {
                            Rotation = 0.5f;
                        }
                        bonuses.Add(AssignEvents(enemy));
                    }
                    hash.Add(enemy);
                }

                if (el.Count == 4) {
                    var sameLine = el[0].GetMatrixPos.Y == el[1].GetMatrixPos.Y;
                    Rotation = sameLine ? 0f : 0.5f;
                    bonuses.Add(AssignEvents(maxTimeEnemy));
                }
            }

            return bonuses;
        }
    }
}
