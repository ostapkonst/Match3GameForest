using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public interface ICollided : IGameObject
    {
        Rectangle GetBounds();
        bool Collide(ICollided p2);
    }
}
