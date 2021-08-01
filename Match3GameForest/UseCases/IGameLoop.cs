using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public interface IGameLoop
    {
        void HandleUpdate(GameInputState state);
    }
}
