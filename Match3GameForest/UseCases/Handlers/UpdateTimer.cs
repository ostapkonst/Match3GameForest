using System;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Match3GameForest.Entities;

namespace Match3GameForest.UseCases
{
    public class UpdateTimer : GameLoop
    {
        private readonly Lazy<GameSettings> _gameSettings;
        private readonly Lazy<ITimer> _timer;

        public UpdateTimer(IContentManager contentManager)
        {
            _gameSettings = contentManager.Get<GameSettings>("settings");
            _timer = contentManager.Get<ITimer>("timer");
        }

        public override void HandleUpdate(GameInputState state)
        {
            var settings = _gameSettings.Value;
            var timer = _timer.Value;

            if (settings.State == GameState.Timed) {
                timer.Restart();
                settings.State = GameState.Play;
            }

            timer.Update(state.GameTime);
            settings.TimeLeft = timer.TimeLeft;

            base.HandleUpdate(state);
        }
    }
}
