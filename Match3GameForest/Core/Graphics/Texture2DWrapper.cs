using Match3GameForest.Config;
using Microsoft.Xna.Framework.Graphics;

namespace Match3GameForest.Core
{
    public class Texture2DWrapper : ITexture2D, IRegistering
    {
        private readonly Texture2D _texture;

        public Texture2DWrapper(Texture2D texture)
        {
            _texture = texture;
        }

        public int Height => _texture.Height;
        public int Width => _texture.Width;
        public Texture2D UnderlyingTexture => _texture;
    }
}