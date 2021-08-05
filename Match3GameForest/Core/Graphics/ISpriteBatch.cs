using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public interface ISpriteBatch
    {
        void Draw(ITexture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale);
        void Draw(ITexture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color);
        void Draw(ITexture2D texture, Rectangle rectangle, Color color);
        void Begin(IScreen screen);
        void End();
    }
}
