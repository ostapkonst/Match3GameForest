using System.Collections.Generic;

namespace Match3GameForest.Entities
{
    public interface IBonusFactory
    {
        FieldSeries Build(FieldSeries gameField);
    }
}
