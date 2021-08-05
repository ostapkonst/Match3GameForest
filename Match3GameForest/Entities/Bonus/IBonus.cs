using System.Collections.Generic;
using Match3GameForest.Core;

namespace Match3GameForest.Entities
{
    public interface IBonus : ISprite
    {
        IList<IBonus> Build(FieldSeries series);
        bool IsActive { get; }
        IEnemy Carrier { get; }
        void Deactivate();
    }
}
