using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public abstract class GameLoop : IGameLoop
    {
        public virtual IGameLoop Next { get; protected set; }

        public virtual void HandleUpdate(GameInputState state)
        {
            Next?.HandleUpdate(state);
        }
    }
}
