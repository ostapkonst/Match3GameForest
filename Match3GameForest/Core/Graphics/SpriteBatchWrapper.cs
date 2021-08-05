using System;
using Match3GameForest.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3GameForest.Core
{
    public class SpriteBatchWrapper : ISpriteBatch, IRegistering
    {
        private readonly SpriteBatch _spriteBatch;

        public SpriteBatchWrapper(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void Draw(ITexture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color)
        {
            var underlyingTexture = ((Texture2DWrapper)texture).UnderlyingTexture;

            _spriteBatch.Draw(underlyingTexture, destinationRectangle, sourceRectangle, color);
        }

        public void Draw(ITexture2D texture, Rectangle rectangle, Color color)
        {
            var underlyingTexture = ((Texture2DWrapper)texture).UnderlyingTexture;

            _spriteBatch.Draw(underlyingTexture, rectangle, color);
        }

        public void Draw(ITexture2D texture, Vector2 position, Rectangle sourceRectangle, Color color, float rotation, Vector2 origin, float scale)
        {
            var underlyingTexture = ((Texture2DWrapper)texture).UnderlyingTexture;

            _spriteBatch.Draw(underlyingTexture, position, sourceRectangle, color, (float)Math.PI * rotation, origin, scale, SpriteEffects.None, 0);
        }

        public void Begin(IScreen screen)
        {
            var screenScale = screen.LocalToWorldScale();
            var scale = Matrix.CreateScale(screenScale.X, screenScale.Y, 1f);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, scale);
        }

        public void End()
        {
            _spriteBatch.End();
        }
    }
}