using System;
using System.Collections.Generic;
using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Entities
{
    public interface IGameField : IGameObject
    {
        void GenerateField(int matrixRows, int matrixColumns);
        int MatrixRows { get; }
        int MatrixColumns { get; }
        IEnemy GetEnemyByVector(Vector2 position);
        bool IsNear(IEnemy first, IEnemy second);
        bool IsSameType(IEnemy first, IEnemy second);
        void SwapEnemies(IEnemy first, IEnemy second);

        IList<IList<IEnemy>> GetSeries();
        int CalcScore(IList<IList<IEnemy>> enemies);
        IList<IEnemy> Match(IList<IList<IEnemy>> enemies);
        void ForSeries(IList<IList<IEnemy>> enemies, Action<IEnemy> action);
        void ForSeries(IList<IEnemy> enemies, Action<IEnemy> action);
    }
}
