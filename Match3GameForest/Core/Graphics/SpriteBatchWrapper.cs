using Match3GameForest.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3GameForest.Core
{
    public class SpriteBatchWrapper : ISpriteBatch, IRegistering
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly IScreen _screen;

        public SpriteBatchWrapper(SpriteBatch spriteBatch, IScreen screen)
        {
            _spriteBatch = spriteBatch;
            _screen = screen;
        }

        public void Draw(ITexture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color)
        {
            var underlyingTexture = ((Texture2DWrapper)texture).UnderlyingTexture;

            _spriteBatch.Draw(underlyingTexture, _screen.Rescale(destinationRectangle), sourceRectangle, color);
        }

        public void Draw(ITexture2D texture, Rectangle rectangle, Color color)
        {
            var underlyingTexture = ((Texture2DWrapper)texture).UnderlyingTexture;

            _spriteBatch.Draw(underlyingTexture, _screen.Rescale(rectangle), color);
        }

        public void Begin()
        {
            _spriteBatch.Begin();
        }

        public void End()
        {
            _spriteBatch.End();
        }
    }
}