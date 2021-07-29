using System;
using Match3GameForest.Config;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public class ScreenWrapper : IScreen, IRegistering
    {
        public Point World { get; private set; }
        public Point Local { get; private set; }

        private Vector2? L2W = null;

        public void Update(GameInputState state, IGameObject gameObject)
        {
            World = state.WindowSize;
            Local = new Point(gameObject.ScaledWidth, gameObject.ScaledHeight);

            L2W = null;
        }

        public Vector2 WorldToLocalScale(bool saveProportion)
        {
            var L2W = LocalToWorldScale(saveProportion);
            var W2L = new Vector2(1) / L2W;

            return W2L;
        }

        public Vector2 LocalToWorldScale(bool saveProportion)
        {
            if (L2W == null) {
                float X = World.X / MathF.Max(Local.X, 1f);
                float Y = World.Y / MathF.Max(Local.Y, 1f);
                L2W = new Vector2(X, Y);
            }

            var tmp = L2W.Value;

            if (saveProportion) {
                tmp = new Vector2(MathF.Min(tmp.X, tmp.Y));
            }

            return tmp;
        }

        public Rectangle Rescale(Rectangle rectangle, bool saveProportion)
        {
            var loc = new Point(
                    (int)(rectangle.X * LocalToWorldScale(saveProportion).X),
                    (int)(rectangle.Y * LocalToWorldScale(saveProportion).Y)
                );

            var size = new Point(
                    (int)(rectangle.Width * LocalToWorldScale(saveProportion).X),
                    (int)(rectangle.Height * LocalToWorldScale(saveProportion).Y)
                );

            return new Rectangle(loc, size);
        }
    }
}
