using Match3GameForest.Core;

namespace Match3GameForest.UseCases
{
    public class WaiteEffect : AnimationWrapper
    {
        private int _elapsedTime;

        public int FrameTime { get; private set; }

        public WaiteEffect(int frameTime) : base()
        {
            FrameTime = frameTime;
            _elapsedTime = 0;
            _finished = false;
        }

        public override void Update(GameInputState state)
        {
            if (!_finished) {
                var milliseconds = (int)state.GameTime.ElapsedGameTime.TotalMilliseconds;

                _elapsedTime += milliseconds;
                if (_elapsedTime >= FrameTime) {
                    _finished = true;
                }
            }

            base.Update(state);
        }
    }
}
