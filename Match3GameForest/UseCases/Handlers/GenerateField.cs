using System;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Match3GameForest.Entities;

namespace Match3GameForest.UseCases
{
    public class GenerateField : GameLoop
    {
        private readonly Lazy<GameSettings> _gameSettings;
        private readonly Lazy<IGameField> _gameField;

        public GenerateField(IContentManager contentManager)
        {
            _gameField = contentManager.Get<IGameField>("field");
            _gameSettings = contentManager.Get<GameSettings>("settings");

            Next = new DisplayField(contentManager);
        }

        public override void HandleUpdate(GameInputState state)
        {
            var settings = _gameSettings.Value;

            if (settings.State == GameState.Init) {
                _gameField.Value.GenerateField(settings.MatrixColumns, settings.MatrixRows);
                settings.GameScore = 0;
                settings.State = GameState.Timed;
            }

            base.HandleUpdate(state);
        }
    }
}
