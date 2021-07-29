using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public interface IScreen
    {
        Point World { get; }
        Point Local { get; }

        Vector2 WorldToLocalScale(bool saveProportion = true);
        Vector2 LocalToWorldScale(bool saveProportion = true);

        void Update(GameInputState state, IGameObject gameObject);
        Rectangle Rescale(Rectangle rectangle, bool saveProportion = true);
    }
}
