using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Entities
{
    public interface IEnemy : ISprite
    {
        int Prize { get; }
        void Destroy();
        bool IsActive { get; }
        bool Selected { get; set; }
        string Type { get; }
        Point MatrixPos { get; set; }
    }
}