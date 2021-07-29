using System;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public interface ISprite : IGameObject
    {
        Vector2 Position { get; set; }
        Rectangle GetBounds();
        bool Paused { get; set; }
        bool Hidden { get; set; }
        Color Lightning { get; set; }
        float Scale { get; set; }
        Guid Id { get; }
    }
}