using System;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Match3GameForest.Entities;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public class DisplayField : IGameLoop
    {
        private readonly IAnimation _animationManager;
        private readonly IGameField _gameField;
        private readonly GameSettings settings;

        public DisplayField(IContentManager contentManager)
        {
            _animationManager = contentManager.Get<IAnimation>("animation");
            _gameField = contentManager.Get<IGameField>("field");
            settings = contentManager.Get<GameSettings>("settings");
        }

        public void HandleUpdate(GameInputState state)
        {
            if (settings.State != GameState.Play) return;

            _animationManager.Update(state); // сначала запускаем циклы анимации
            _gameField.Update(state);
        }
    }
}
