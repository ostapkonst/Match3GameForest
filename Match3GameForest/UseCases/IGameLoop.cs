using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public interface IGameLoop
    {
        IGameLoop Next { get; }
        void HandleUpdate(GameInputState state);
    }
}
