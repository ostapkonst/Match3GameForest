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
        private List<IList<IEnemy>> _series;

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
            _series = new List<IList<IEnemy>>();
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

        public IList<IList<IEnemy>> GetSeries()
        {
            if (_updateSeries) return _series;

            _series.Clear();

            _series.AddRange(GetMatchByCol());
            _series.AddRange(GetMatchByRow());

            _updateSeries = true;

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

        public IList<IEnemy> Match(IList<IList<IEnemy>> enemies)
        {
            var createdEnemies = new List<IEnemy>();

            if (enemies.Count == 0) return createdEnemies;

            foreach (var el in enemies) {
                foreach (var enemy in el) {
                    enemy.Destroy();
                }
            }

            for (var col = 0; col < MatrixColumns; col++) {
                int shift = 0;
                for (var row = MatrixRows - 1; row >= 0; row--) {
                    var enemy = FieldMatrix[row, col];
                    if (!enemy.IsActive) {
                        shift++;
                    } else {
                        SetPos(enemy, col, row + shift);
                    }
                }
                for (var row = 0; row < shift; row++) {
                    var en = _enemyFactory.Build();
                    SetPos(en, col, row);
                    createdEnemies.Add(en);
                }
            }

            _updateSeries = false;
            return createdEnemies;
        }

        public int CalcScore(IList<IList<IEnemy>> enemies)
        {
            var set = new HashSet<IEnemy>();

            foreach (var el in enemies) {
                foreach (var enemy in el) {
                    set.Add(enemy);
                }
            }

            return set.Sum(x => x.Prize);
        }

        public void ForSeries(IList<IList<IEnemy>> enemies, Action<IEnemy> action)
        {
            var set = new HashSet<IEnemy>();

            foreach (var el in enemies) {
                foreach (var enemy in el) {
                    set.Add(enemy);
                }
            }

            foreach (var el in set) {
                action(el);
            }
        }

        public void ForSeries(IList<IEnemy> enemies, Action<IEnemy> action)
        {
            var set = new HashSet<IEnemy>();

            foreach (var enemy in enemies) {
                set.Add(enemy);
            }

            foreach (var el in set) {
                action(el);
            }
        }
    }
}
