using System;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public interface ISprite : IGameObject, ICloneable
    {
        Vector2 Position { get; set; }
        bool Paused { get; set; }
        bool Hidden { get; set; }
        Color Lightning { get; set; }
        float Scale { get; set; }
        Guid Id { get; }

        event Action<GameInputState> BeforeUpdate;
        event Action<GameInputState> AfterUpdate;
        event Action<GameTime> BeforeDraw;
        event Action<GameTime> AfterDraw;

        event Action<Vector2> OnPosition;
        event Action<float> OnScale;
        event Action<Color> OnLightning;
    }
}