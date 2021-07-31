using Match3GameForest.Entities;
using Match3GameForest.Core;
using System;

namespace Match3GameForest.UseCases
{
    public class SwapTwoEnemies : GameLoop
    {
        private IEnemy _firstSelEnemy;
        private IEnemy _secondSelEnemy;
        private readonly IAnimation _animationManager;
        private readonly IGameField _gameField;

        public SwapTwoEnemies(IContentManager contentManager)
        {
            _animationManager = contentManager.Get<IAnimation>("animation");
            _gameField = contentManager.Get<IGameField>("field");

            Next = new DestroyEnemies(contentManager);
        }

        private void TryToSelect(GameInputState state, IGameField gameField)
        {
            if (!state.isMouseClicked) return;

            var enemy = gameField.GetEnemyByVector(state.CursorPosition);

            if (enemy != null) {
                if (_firstSelEnemy == null) {
                    _firstSelEnemy = enemy;
                    _firstSelEnemy.Selected = true;
                } else {
                    if (enemy != _firstSelEnemy) {
                        _secondSelEnemy = enemy;
                    }
                }
            }
        }

        private void SwapAnimation(IAnimation animation, IEnemy enemy1, IEnemy enemy2, Action next)
        {
            var wrap1 = new AnimationWrapper();

            var act1 = new MoveToEffect(enemy1, enemy2.Position, 400);
            var act2 = new MoveToEffect(enemy2, enemy1.Position, 400);

            wrap1.Add(act1).Add(act2).AfterAnimate = next;

            animation.Add(wrap1);
        }

        private void ReturnBack(IAnimation animation, IGameField gameField, IEnemy enemy1, IEnemy enemy2)
        {
            SwapAnimation(animation, enemy1, enemy2, () =>
            {
                gameField.SwapEnemies(enemy2, enemy1);
            });
        }

        private void SwapSelected(IAnimation animation, IGameField gameField, IEnemy enemy1, IEnemy enemy2)
        {
            if (!gameField.IsNear(enemy1, enemy2)) return;

            SwapAnimation(animation, enemy1, enemy2, () =>
            {
                gameField.SwapEnemies(enemy1, enemy2);

                if (gameField.GetSeries().Count == 0) {
                    ReturnBack(animation, gameField, enemy1, enemy2);
                }
            });
        }

        private void RemoveSelection()
        {
            _firstSelEnemy.Selected = false;
            _firstSelEnemy = _secondSelEnemy = null;
        }

        public override void HandleUpdate(GameInputState state)
        {
            TryToSelect(state, _gameField);

            if (_firstSelEnemy != null && _secondSelEnemy != null) {
                SwapSelected(_animationManager, _gameField, _firstSelEnemy, _secondSelEnemy);

                RemoveSelection();
            }

            base.HandleUpdate(state);
        }
    }
}
