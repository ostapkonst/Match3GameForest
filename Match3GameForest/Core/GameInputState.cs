using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Match3GameForest.Core
{
    public class GameInputState
    {
        public GameInputState(MouseState currentMouseState,
            MouseState previousMouseState,
            GameTime gameTime,
            bool blockInput, Point size, Vector2 scale)
        {
            GameTime = gameTime;
            WindowSize = size;
            isMouseClicked = previousMouseState.LeftButton == ButtonState.Pressed
                && currentMouseState.LeftButton == ButtonState.Released && !blockInput;
            CursorPosition = currentMouseState.Position.ToVector2() * scale;
        }

        public Point WindowSize { get; private set; }
        public Vector2 CursorPosition { get; private set; }
        public bool isMouseClicked { get; private set; }
        public GameTime GameTime { get; private set; }
    }
}