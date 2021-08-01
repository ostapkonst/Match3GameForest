using System;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Entities
{
    public interface ITimer
    {
        void Restart(int seconds);
        bool IsActive { get; }
        int TimeLeft { get; }
        void Update(GameTime gameTime);

        event Action OnFinish;
        event Action OnStart;
    }
}
