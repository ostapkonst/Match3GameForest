using Match3GameForest.Config;
using Microsoft.Xna.Framework.Graphics;

namespace Match3GameForest.Core
{
    public class Texture2DWrapper : ITexture2D, IRegistering
    {
        public Texture2DWrapper(Texture2D texture)
        {
            UnderlyingTexture = texture;
        }

        public int Height => UnderlyingTexture.Height;
        public int Width => UnderlyingTexture.Width;
        public Texture2D UnderlyingTexture { get; }
    }
}