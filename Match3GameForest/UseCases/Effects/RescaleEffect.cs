using Match3GameForest.Core;

namespace Match3GameForest.UseCases
{
    public class RescaleEffect : AnimationWrapper
    {
        private int _elapsedTime;
        private readonly float _scale;

        public ISprite Enemy { get; private set; }
        public float FromScale { get; private set; }
        public float ToScale { get; private set; }
        public int FrameTime { get; private set; }

        public RescaleEffect(ISprite sprite, float fromScale, float toScale, int frameTime) : base()
        {
            FrameTime = frameTime;
            FromScale = fromScale;
            Enemy = sprite;
            ToScale = toScale * Enemy.Scale;
            Enemy.Scale *= FromScale;
            _scale = (ToScale - Enemy.Scale) / FrameTime;
            _elapsedTime = 0;
            _finished = false;
        }

        public override void Update(GameInputState state)
        {
            if (!_finished) {
                var milliseconds = (int)state.GameTime.ElapsedGameTime.TotalMilliseconds;

                _elapsedTime += milliseconds;
                if (_elapsedTime >= FrameTime) {
                    Enemy.Scale = ToScale;
                    _finished = true;
                } else {
                    Enemy.Scale += _scale * milliseconds;
                }
            }

            base.Update(state);
        }
    }
}
