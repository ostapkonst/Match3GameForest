using System;
using System.Collections.Generic;
using System.Linq;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Entities
{
    public class GameFieldWrapper : IGameField, IRegistering
    {
        public IEnemy[,] FieldMatrix { get; private set; }

        private readonly IEnemyFactory _enemyFactory;
        private bool _updateSeries;
        private FieldSeries _series;

        public int MatrixRows { get; private set; }
        public int MatrixColumns { get; private set; }

        public int ScaledWidth => MatrixColumns * _blankEnemy.ScaledWidth;
        public int ScaledHeight => MatrixRows * _blankEnemy.ScaledHeight;

        private readonly IEnemy _blankEnemy;

        public GameFieldWrapper(IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
            MatrixRows = MatrixColumns = 0;
            GenerateField(MatrixRows, MatrixColumns);
            _blankEnemy = _enemyFactory.Build();
            _blankEnemy.Destroy();
        }

        public void GenerateField(int matrixRows, int matrixColumns)
        {
            MatrixRows = matrixRows;
            MatrixColumns = matrixColumns;
            FillMatrix();
            _updateSeries = false;
            _series = new FieldSeries();
        }

        private void FillMatrix()
        {
            FieldMatrix = new IEnemy[MatrixRows, MatrixColumns];

            for (var row = 0; row < MatrixRows; row++) {
                for (var col = 0; col < MatrixColumns; col++) {
                    var enemy = _enemyFactory.Build();
                    SetPos(enemy, col, row);
                }
            }
        }

        public IEnemy GetEnemyByVector(Vector2 position)
        {
            foreach (var enemy in FieldMatrix) {
                if (enemy.GetBounds().Contains(position)) {
                    if (!enemy.IsActive) return null;
                    return enemy;
                }
            }

            return null;
        }

        public bool IsNear(IEnemy first, IEnemy second)
        {
            if (!first.IsActive || !second.IsActive) return false;
            return (Math.Abs(first.MatrixPos.X - second.MatrixPos.X) +
                Math.Abs(first.MatrixPos.Y - second.MatrixPos.Y)) <= 1;
        }

        public bool IsSameType(IEnemy first, IEnemy second)
        {
            if (!first.IsActive || !second.IsActive) return false;
            return first.Type == second.Type;
        }

        private IList<IList<IEnemy>> GetMatch(IList<IEnemy> list)
        {
            var tmp = new List<IEnemy>();
            var result = new List<IList<IEnemy>>();

            foreach (var el in list) {
                if (tmp.Count == 0 || !IsSameType(tmp.Last(), el)) {
                    tmp.Clear();
                }
                tmp.Add(el);
                if (tmp.Count == 3) {
                    result.Add(tmp.ToList());
                }
                if (tmp.Count > 3) {
                    result.Last().Add(el);
                }
            }

            return result;
        }

        private IList<IList<IEnemy>> GetMatchByRow()
        {
            var tmp = new List<IEnemy>();
            var result = new List<IList<IEnemy>>();

            for (var row = 0; row < MatrixRows; row++) {
                tmp.Clear();
                for (var col = 0; col < MatrixColumns; col++) {
                    tmp.Add(FieldMatrix[row, col]);
                }
                result.AddRange(GetMatch(tmp));
            }

            return result;
        }

        private IList<IList<IEnemy>> GetMatchByCol()
        {
            var tmp = new List<IEnemy>();
            var result = new List<IList<IEnemy>>();

            for (var col = 0; col < MatrixColumns; col++) {
                tmp.Clear();
                for (var row = 0; row < MatrixRows; row++) {
                    tmp.Add(FieldMatrix[row, col]);
                }
                result.AddRange(GetMatch(tmp));
            }

            return result;
        }

        public FieldSeries GetSeries()
        {
            if (!_updateSeries) {
                var series = new List<IList<IEnemy>>();

                series.AddRange(GetMatchByCol());
                series.AddRange(GetMatchByRow());

                _series = new FieldSeries(series);
                _updateSeries = true;
            }

            return _series;
        }

        public void SwapEnemies(IEnemy first, IEnemy second)
        {
            FieldMatrix[first.MatrixPos.Y, first.MatrixPos.X] = second;
            FieldMatrix[second.MatrixPos.Y, second.MatrixPos.X] = first;

            var tmpMPos = first.MatrixPos;
            first.MatrixPos = second.MatrixPos;
            second.MatrixPos = tmpMPos;

            _updateSeries = false;
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var enemy in FieldMatrix) {
                enemy.Draw(gameTime);
            }
        }

        public void Update(GameInputState state)
        {
            foreach (var enemy in FieldMatrix) {
                enemy.Update(state);
            }
        }

        private void SetPos(IEnemy enemy, int X, int Y)
        {
            var bounds = enemy.GetBounds();
            var posX = X * bounds.Width + _blankEnemy.ScaledWidth / 2;
            var posY = Y * bounds.Height + _blankEnemy.ScaledHeight / 2;
            enemy.Position = new Vector2(posX, posY);
            enemy.MatrixPos = new Point(X, Y);
            FieldMatrix[Y, X] = enemy;
        }

        public FieldSeries Match(FieldSeries enemies)
        {
            var createdEnemies = new List<IEnemy>();

            if (enemies.IsEmpty) return new FieldSeries(createdEnemies);

            foreach (var enemy in enemies) {
                enemy.Destroy();
            }

            var shifts = new List<int>();
            var moves = new List<Tuple<IEnemy, Point>>();

            for (var col = 0; col < MatrixColumns; col++) {
                int shift = 0;
                for (var row = MatrixRows - 1; row >= 0; row--) {
                    var enemy = FieldMatrix[row, col];
                    if (!enemy.IsActive) {
                        shift++;
                    } else {
                        var point = new Point(row + shift, col);
                        moves.Add(new Tuple<IEnemy, Point>(enemy, point));
                    }
                }
                shifts.Add(shift);
            }

            foreach (var move in moves) {
                var pos = move.Item2;
                SetPos(move.Item1, pos.Y, pos.X);
            }

            for (var col = 0; col < MatrixColumns; col++) {
                for (var row = 0; row < shifts[col]; row++) {
                    var en = _enemyFactory.Build();
                    SetPos(en, col, row);
                    createdEnemies.Add(en);
                }
            }

            _updateSeries = false;
            return new FieldSeries(createdEnemies);
        }
    }
}
