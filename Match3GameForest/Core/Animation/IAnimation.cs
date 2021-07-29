using System;

namespace Match3GameForest.Core
{
    public interface IAnimation
    {
        bool IsAnimate { get; }
        IAnimation Add(IAnimation animation);
        void Update(GameInputState state);
        Action AfterAnimate { get; set; }
    }
}
