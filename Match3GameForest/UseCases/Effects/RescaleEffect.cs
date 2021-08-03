using Match3GameForest.Core;

namespace Match3GameForest.UseCases
{
    public class RescaleEffect : AnimationWrapper
    {
        private readonly ISprite _enemy;
        private int _elapsedTime;
        private readonly double _scale;

        public float FromScale { get; set; }
        public float ToScale { get; set; }

        public int FrameTime { get; set; }

        public RescaleEffect(ISprite sprite, float fromScale, float toScale, int frameTime) : base()
        {
            FrameTime = frameTime;
            FromScale = fromScale;
            _enemy = sprite;
            ToScale = toScale * _enemy.Scale;
            _enemy.Scale *= FromScale;
            _scale = (ToScale - _enemy.Scale) / FrameTime;
            _elapsedTime = 0;
            _finished = false;
        }

        public override void Update(GameInputState state)
        {
            if (!_finished) {
                var milliseconds = (int)state.GameTime.ElapsedGameTime.TotalMilliseconds;

                _elapsedTime += milliseconds;
                if (_elapsedTime >= FrameTime) {
                    _enemy.Scale = ToScale;
                    _finished = true;
                } else {
                    _enemy.Scale += (float)_scale * milliseconds;
                }
            }

            base.Update(state);
        }
    }
}
