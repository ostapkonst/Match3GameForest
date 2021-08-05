using System;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public interface ISprite : IGameObject
    {
        Guid Id { get; }

        Color Lightning { get; set; }
        float Scale { get; set; }
        float Rotation { get; set; }
        
        event Action<GameInputState> BeforeUpdate;
        event Action<GameInputState> AfterUpdate;
        event Action<GameTime> BeforeDraw;
        event Action<GameTime> AfterDraw;

        event Action<Vector2> OnPosition;
        event Action<float> OnScale;
        event Action<Color> OnLightning;
        event Action<float> OnRotation;
    }
}