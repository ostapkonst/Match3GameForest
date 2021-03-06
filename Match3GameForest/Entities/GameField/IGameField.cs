using System;
using System.Collections.Generic;
using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Entities
{
    public interface IGameField : ICollided
    {
        void GenerateField(int matrixRows, int matrixColumns);

        int MatrixRows { get; }
        int MatrixColumns { get; }

        void RefreshField();

        IEnemy GetEnemyByVector(Vector2 position);
        bool IsNear(IEnemy first, IEnemy second);
        bool IsSameType(IEnemy first, IEnemy second);
        void SwapEnemies(IEnemy first, IEnemy second);
        void DestroyEnemies(FieldSeries enemies);

        FieldSeries GetField();
        FieldSeries GetMatchSeries();

        int Score { get; }
        void Match();

        event Action<IList<IEnemy>> OnDestroy;
        event Action<IList<Tuple<IEnemy, Vector2>>> OnMove;
        event Action<IList<IEnemy>> OnCreate;
    }
}
