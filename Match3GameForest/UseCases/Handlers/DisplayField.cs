using System;
using Match3GameForest.Core;
using Match3GameForest.Entities;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public class DisplayField : GameLoop
    {
        private readonly IAnimation _animateManager;
        private readonly IGameField _gameField;

        public DisplayField(IContentManager contentManager)
        {
            _animateManager = contentManager.Get<IAnimation>("animation");
            _gameField = contentManager.Get<IGameField>("field");

            Next = new SwapTwoEnemies(contentManager);
        }

        public override void HandleUpdate(GameInputState state)
        {
            _animateManager.Update(state); // сначала запускаем циклы анимации

            _gameField.Update(state);

            base.HandleUpdate(state);
        }

        public override void HandleDraw(GameTime gameTime)
        {
            _gameField.Draw(gameTime);

            base.HandleDraw(gameTime);
        }
    }
}
