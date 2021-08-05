using System.Diagnostics;
using Match3GameForest.Core;
using Match3GameForest.Entities;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public class MoveDirectionEffect : AnimationWrapper
    {
        public Destroyer Enemy { get; private set; }
        public ICollided Collider { get; private set; }
        public int Speed { get; private set; }

        public MoveDirectionEffect(Destroyer enemy, int speed, ICollided collider) : base()
        {
            Speed = speed;
            Enemy = enemy;
            Collider = collider;
            _finished = false;
        }

        private bool Inside(Rectangle p1, Rectangle p2)
        {
            return p1.Left >= p2.Left
                && p1.Right <= p2.Right
                && p1.Top >= p2.Top
                && p1.Bottom <= p2.Bottom;
        }

        public override void Update(GameInputState state)
        {
            if (!_finished) {
                var milliseconds = (int)state.GameTime.ElapsedGameTime.TotalMilliseconds;
                float deltaTime = (float)(milliseconds / 1000.0f);

                if (!Inside(Enemy.GetBounds(), Collider.GetBounds())) {
                    _finished = true;
                } else {
                    Enemy.Position += Enemy.Direction * Speed * deltaTime;
                }
            }

            base.Update(state);
        }
    }
}
