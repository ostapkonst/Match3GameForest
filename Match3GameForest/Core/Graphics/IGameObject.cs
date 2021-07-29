using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public interface IGameObject
    {
        void Draw(GameTime gameTime);
        void Update(GameInputState state);
        int ScaledWidth { get; }
        int ScaledHeight { get; }
    }
}
