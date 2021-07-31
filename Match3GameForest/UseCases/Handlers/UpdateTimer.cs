using System;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Match3GameForest.Entities;

namespace Match3GameForest.UseCases
{
    public class UpdateTimer : GameLoop
    {
        private readonly GameSettings _gameSettings;
        private readonly ITimer _timer;

        public UpdateTimer(IContentManager contentManager)
        {
            _gameSettings = contentManager.Get<GameSettings>("settings");
            _timer = contentManager.Get<ITimer>("timer");
        }

        public override void HandleUpdate(GameInputState state)
        {
            if (_gameSettings.State == GameState.Timed) {
                _timer.Restart();
                _gameSettings.State = GameState.Play;
            }

            _timer.Update(state.GameTime);
            _gameSettings.TimeLeft = _timer.TimeLeft;

            base.HandleUpdate(state);
        }
    }
}
