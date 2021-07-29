using System;
using Match3GameForest.Core;
using Match3GameForest.Entities;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public class DisplayField : GameLoop
    {
        private readonly Lazy<IAnimation> _animateManager;
        private readonly Lazy<IGameField> _gameField;

        public DisplayField(IContentManager contentManager)
        {
            _animateManager = contentManager.Get<IAnimation>("animation");
            _gameField = contentManager.Get<IGameField>("field");

            Next = new SwapTwoEnemies(contentManager);
        }

        public override void HandleUpdate(GameInputState state)
        {
            _animateManager.Value.Update(state); // сначала запускаем циклы анимации

            _gameField.Value.Update(state);

            base.HandleUpdate(state);
        }

        public override void HandleDraw(GameTime gameTime)
        {
            _gameField.Value.Draw(gameTime);

            base.HandleDraw(gameTime);
        }
    }
}
