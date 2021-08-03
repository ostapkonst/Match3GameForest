using System.Collections.Generic;

namespace Match3GameForest.Entities
{
    public interface IBonusFactory
    {
        float Scale { get; set; }
        FieldSeries Build(FieldSeries gameField);
        bool IsActivate { get; }
        List<IBonus> GetActiveBonuses();
        void Clear();
    }
}
