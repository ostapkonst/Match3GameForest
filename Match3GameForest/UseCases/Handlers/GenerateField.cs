using System;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Match3GameForest.Entities;

namespace Match3GameForest.UseCases
{
    public class GenerateField : GameLoop
    {
        private readonly GameSettings _gameSettings;
        private readonly IGameField _gameField;

        public GenerateField(IContentManager contentManager)
        {
            _gameField = contentManager.Get<IGameField>("field");
            _gameSettings = contentManager.Get<GameSettings>("settings");

            Next = new DisplayField(contentManager);
        }

        public override void HandleUpdate(GameInputState state)
        {
            if (_gameSettings.State == GameState.Init) {
                _gameField.GenerateField(_gameSettings.MatrixColumns, _gameSettings.MatrixRows);
                _gameSettings.GameScore = 0;
                _gameSettings.State = GameState.Timed;
            }

            base.HandleUpdate(state);
        }
    }
}
