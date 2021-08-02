﻿using System;
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

        public FieldSeries GetSeries()
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
            enemy.MatrixPos = new Point(X, Y);
            enemy.Position = CalctPos(enemy, enemy.MatrixPos);
            FieldMatrix[Y, X] = enemy;
        }

        private void SetPos(IEnemy enemy, Point pos)
        {
            SetPos(enemy, pos.X, pos.Y);
        }

        private Vector2 CalctPos(IEnemy enemy, Point coord)
        {
            var bounds = enemy.GetBounds();
            var posX = (coord.X * bounds.Width) + (enemy.ScaledWidth / 2);
            var posY = (coord.Y * bounds.Height) + (enemy.ScaledHeight / 2);
            return new Vector2(posX, posY);
        }

        public void Match()
        {
            var enemies = GetSeries();

            if (enemies.IsEmpty) return;

            var bonus = _bonusManager.Build(enemies);

            // Удаляем врагов
            OnDestroy?.Invoke(enemies.Line);

            foreach (var enemy in enemies) {
                enemy.Destroy();
            }

            // Добавляем бонусы
            foreach (var enemy in bonus) {
                SetPos(enemy, enemy.MatrixPos);
            }

            if (!bonus.IsEmpty) {
                OnCreate?.Invoke(bonus.Line);
            }

            // Враги падают на пустые клетки
            var shifts = new List<int>();
            var moves = new List<Tuple<IEnemy, Point>>();
            for (var col = 0; col < MatrixColumns; col++) {
                int shift = 0;
                for (var row = MatrixRows - 1; row >= 0; row--) {
                    var enemy = FieldMatrix[row, col];
                    if (!enemy.IsActive) {
                        shift++;
                    } else {
                        if (shift > 0) {
                            var point = new Point(col, row + shift);
                            moves.Add(new Tuple<IEnemy, Point>(enemy, point));
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

            // Создаем новых врагов
            var createdEnemies = new List<IEnemy>();
            for (var col = 0; col < MatrixColumns; col++) {
                for (var row = 0; row < shifts[col]; row++) {
                    var en = _enemyFactory.Build();
                    SetPos(en, col, row);
                    createdEnemies.Add(en);
                }
            }

            OnCreate?.Invoke(createdEnemies);

            _updateSeries = false;
        }

        public int Score => GetSeries().Score;
    }
}