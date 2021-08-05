using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public interface IGameObject
    {
        bool Hidden { get; set; }

        void Draw(GameTime gameTime);
        void Update(GameInputState state);

        int ScaledWidth { get; }
        int ScaledHeight { get; }

        int FrameWidth { get; }
        int FrameHeight { get; }

        Vector2 Position { get; set; }
    }
}
