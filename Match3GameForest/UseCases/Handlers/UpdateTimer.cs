using Match3GameForest.Config;
using Match3GameForest.Core;

namespace Match3GameForest.UseCases
{
    public class UpdateTimer : IGameLoop
    {
        private readonly GameSettings _settings;
        private readonly IAnimation _animationManager;
        private readonly ITimer _timer;

        public UpdateTimer(IContentManager contentManager)
        {
            _animationManager = contentManager.Get<IAnimation>("animation");
            _settings = contentManager.Get<GameSettings>("settings");
            _timer = contentManager.Get<ITimer>("timer");
        }

        public void HandleUpdate(GameInputState state)
        {
            if (_settings.State == GameState.Timed) {
                _timer.Restart(_settings.PlayingDuration);
                _settings.State = GameState.Play;
            }

            if (_settings.State == GameState.Play) {
                if (_animationManager.IsAnimate) return; // Пауза во время анимации
                _timer.Update(state.GameTime);
                _settings.TimeLeft = _timer.TimeLeft;
            }
        }
    }
}
