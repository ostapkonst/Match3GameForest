using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IBonusFactory _bonusManager;
        private bool _updateSeries;
        private FieldSeries _series;

        public int MatrixRows { get; private set; }
        public int MatrixColumns { get; private set; }

        public int ScaledWidth => MatrixColumns * _blankEnemy.ScaledWidth;
        public int ScaledHeight => MatrixRows * _blankEnemy.ScaledHeight;

        private readonly IEnemy _blankEnemy;

        public event Action<IList<IEnemy>> OnDestroy;
        public event Action<IList<Tuple<IEnemy, Vector2>>> OnMove;
        public event Action<IList<IEnemy>> OnCreate;

        public GameFieldWrapper(IEnemyFactory enemyFactory, IBonusFactory bonusManager)
        {
            _enemyFactory = enemyFactory;
            _bonusManager = bonusManager;
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
            _bonusManager.Clear();
            _updateSeries = false;
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
            return (Math.Abs(first.GetMatrixPos.X - second.GetMatrixPos.X) +
                Math.Abs(first.GetMatrixPos.Y - second.GetMatrixPos.Y)) <= 1;
        }

        public bool IsSameType(IEnemy first, IEnemy second)
        {
            if (!first.IsActive || !second.IsActive) return false;
            return first.Type == second.Type;
        }

        private FieldSeries GetMatch(IList<IEnemy> list)
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

            return new FieldSeries(result);
        }

        private FieldSeries GetMatchByRow()
        {
            var tmp = new List<IEnemy>();
            var result = new List<IList<IEnemy>>();

            for (var row = 0; row < MatrixRows; row++) {
                tmp.Clear();
                for (var col = 0; col < MatrixColumns; col++) {
                    tmp.Add(FieldMatrix[row, col]);
                }
                result.AddRange(GetMatch(tmp).Series);
            }

            return new FieldSeries(result);
        }

        private FieldSeries GetMatchByCol()
        {
            var tmp = new List<IEnemy>();
            var result = new List<IList<IEnemy>>();

            for (var col = 0; col < MatrixColumns; col++) {
                tmp.Clear();
                for (var row = 0; row < MatrixRows; row++) {
                    tmp.Add(FieldMatrix[row, col]);
                }
                result.AddRange(GetMatch(tmp).Series);
            }

            return new FieldSeries(result);
        }

        public FieldSeries GetMatchSeries()
        {
            if (!_updateSeries) {
                var series = new List<IList<IEnemy>>();

                series.AddRange(GetMatchByCol().Series);
                series.AddRange(GetMatchByRow().Series);

                _series = new FieldSeries(series);
                _updateSeries = true;
            }

            return _series;
        }

        public void SwapEnemies(IEnemy first, IEnemy second)
        {
            var p1 = first.GetMatrixPos;
            var p2 = second.GetMatrixPos;

            FieldMatrix[p1.Y, p1.X] = second;
            FieldMatrix[p2.Y, p2.X] = first;

            var tmpMPos = first.GetMatrixPos;
            first.GetMatrixPos = second.GetMatrixPos;
            second.GetMatrixPos = tmpMPos;

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
            var newPos = new MatrixPos(X, Y);

            enemy.GetMatrixPos = newPos;
            enemy.Position = CalctPos(enemy, enemy.GetMatrixPos);
            FieldMatrix[Y, X] = enemy;

            _updateSeries = false;
        }

        private void SetPos(IEnemy enemy, MatrixPos pos)
        {
            SetPos(enemy, pos.X, pos.Y);
        }

        private Vector2 CalctPos(IEnemy enemy, MatrixPos coord)
        {
            var bounds = enemy.GetBounds();
            var posX = (coord.X * bounds.Width) + (enemy.ScaledWidth / 2);
            var posY = (coord.Y * bounds.Height) + (enemy.ScaledHeight / 2);
            return new Vector2(posX, posY);
        }

        public void Match()
        {
            var enemies = GetMatchSeries();

            if (enemies.IsEmpty) return;

            var bonuses = _bonusManager.Build(enemies);

            DestroyEnemies(enemies);

            AddBonuses(bonuses);
        }

        public void RefreshField()
        {
            var shifts = new List<int>();
            var moves = new List<Tuple<IEnemy, MatrixPos>>();
            for (var col = 0; col < MatrixColumns; col++) {
                int shift = 0;
                for (var row = MatrixRows - 1; row >= 0; row--) {
                    var enemy = FieldMatrix[row, col];
                    if (!enemy.IsActive) {
                        shift++;
                    } else {
                        if (shift > 0) {
                            var point = new MatrixPos(col, row + shift);
                            moves.Add(new Tuple<IEnemy, MatrixPos>(enemy, point));
                        }
                    }
                }
                shifts.Add(shift);
            }

            var movesVectored = new List<Tuple<IEnemy, Vector2>>();
            foreach (var move in moves) {
                var pos = CalctPos(move.Item1, move.Item2);
                var tuple = new Tuple<IEnemy, Vector2>(move.Item1, pos);
                movesVectored.Add(tuple);
            }

            if (movesVectored.Count > 0) {
                OnMove?.Invoke(movesVectored);
            }

            foreach (var move in moves) {
                SetPos(move.Item1, move.Item2);
            }

            var createdEnemies = new List<IEnemy>();
            for (var col = 0; col < MatrixColumns; col++) {
                for (var row = 0; row < shifts[col]; row++) {
                    var en = _enemyFactory.Build();
                    SetPos(en, col, row);
                    createdEnemies.Add(en);
                }
            }

            if (createdEnemies.Count > 0) {
                OnCreate?.Invoke(createdEnemies);
            }
        }

        public void DestroyEnemies(FieldSeries enemies)
        {
            OnDestroy?.Invoke(enemies.Line);

            foreach (var enemy in enemies) {
                enemy.Destroy();
            }
        }

        private void AddBonuses(FieldSeries bonus)
        {
            foreach (var enemy in bonus) {
                SetPos(enemy, enemy.GetMatrixPos);
            }

            if (!bonus.IsEmpty) {
                OnCreate?.Invoke(bonus.Line);
            }
        }

        public FieldSeries GetField()
        {
            var field = new List<IList<IEnemy>>();

            for (var row = 0; row < MatrixRows; row++) {
                var line = new List<IEnemy>();
                for (var col = 0; col < MatrixColumns; col++) {
                    line.Add(FieldMatrix[row, col]);
                }
                field.Add(line);
            }

            return new FieldSeries(field);
        }

        public int Score => GetMatchSeries().Score;
    }
}
