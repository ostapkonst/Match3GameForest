using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public class MoveToEffect : AnimationWrapper
    {
        private readonly Vector2 _direction;
        private int _elapsedTime;

        public IGameObject Enemy { get; private set; }
        public Vector2 Destination { get; private set; }
        public int FrameTime { get; private set; }

        public MoveToEffect(IGameObject sprite, Vector2 destination, int frameTime) : base()
        {
            Destination = destination;
            FrameTime = frameTime;
            Enemy = sprite;
            _elapsedTime = 0;
            _direction = (Destination - Enemy.Position) / FrameTime;
            _finished = false;
        }

        public override void Update(GameInputState state)
        {
            if (!_finished) {
                var milliseconds = (int)state.GameTime.ElapsedGameTime.TotalMilliseconds;

                _elapsedTime += milliseconds;
                if (_elapsedTime >= FrameTime) {
                    Enemy.Position = Destination;
                    _finished = true;
                } else {
                    Enemy.Position += _direction * milliseconds;
                }
            }

            base.Update(state);
        }
    }
}
