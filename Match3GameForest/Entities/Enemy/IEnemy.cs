using System;
using Match3GameForest.Core;

namespace Match3GameForest.Entities
{
    public interface IEnemy : ISprite, ICollided, ICloneable
    {
        int Prize { get; }
        void Destroy();
        
        bool IsActive { get; }
        bool Selected { get; set; }
        string Type { get; }
        DateTime TouchedTime { get; }
        MatrixPos GetMatrixPos { get; set; }

        event Action OnDestroy;
    }
}