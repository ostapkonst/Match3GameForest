using System.Collections.Generic;
using Match3GameForest.Core;

namespace Match3GameForest.Entities
{
    public interface IBonus : ISprite
    {
        FieldSeries Build(FieldSeries series);
    }
}
