using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Entities
{
    public class Destroyer : Sprite, ICollided
    {
        public Destroyer(ISpriteBatch spriteBatch, ITexture2D spriteStrip)
            : base(spriteBatch, spriteStrip)
        {
        }

        public void Destroy()
        {
            Hidden = true;
        }

        public Vector2 Direction { get; set; }

        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)Position.X - ScaledWidth / 2,
                (int)Position.Y - ScaledHeight / 2,
                ScaledWidth,
                ScaledHeight);
        }

        public bool Collide(ICollided p2)
        {
            var r1 = GetBounds();
            var r2 = p2.GetBounds();

            return r1.Intersects(r2);
        }
    }
}
