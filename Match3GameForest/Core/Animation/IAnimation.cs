using System;

namespace Match3GameForest.Core
{
    public interface IAnimation : IDisposable
    {
        bool IsAnimate { get; }
        IAnimation Add(IAnimation animation);
        void Update(GameInputState state);
        void Waite(); // Останавливает вызывающий поток, поэтому вызывать из UseCase/Handlers
    }
}
