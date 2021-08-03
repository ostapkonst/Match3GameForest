using System.Collections.Generic;
using Match3GameForest.Core;

namespace Match3GameForest.Entities
{
    public enum BonusStatus
    {
        Delivered,
        Activated,
        Worked,
        Finished
    }

    public interface IBonus : ISprite
    {
        IList<IBonus> Build(FieldSeries series);
        BonusStatus Status { get; set; }
        IEnemy Carrier { get; }
    }
}
