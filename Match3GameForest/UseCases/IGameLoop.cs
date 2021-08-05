using Match3GameForest.Core;

namespace Match3GameForest.UseCases
{
    public interface IGameLoop
    {
        void HandleUpdate(GameInputState state);
    }
}
