using System.Collections.Generic;
using Match3GameForest.Core;

namespace Match3GameForest.Entities
{
    public interface IBonus : ISprite
    {
        IList<IBonus> Build(FieldSeries series);
        bool IsActivate { get; set; }
        IEnemy Carrier { get; }
    }
}
