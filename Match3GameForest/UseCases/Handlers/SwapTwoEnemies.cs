using Match3GameForest.Entities;
using Match3GameForest.Core;
using Match3GameForest.Config;

namespace Match3GameForest.UseCases
{
    public class SwapTwoEnemies : IGameLoop
    {
        private IEnemy _firstSelEnemy;
        private IEnemy _secondSelEnemy;
        private readonly IAnimation _animationManager;
        private readonly IGameField _gameField;
        private readonly GameSettings _settings;

        public SwapTwoEnemies(IContentManager contentManager)
        {
            _animationManager = contentManager.Get<IAnimation>("animation");
            _gameField = contentManager.Get<IGameField>("field");
            _settings = contentManager.Get<GameSettings>("settings");
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

        private void SwapAnimation(IEnemy enemy1, IEnemy enemy2)
        {
            var wrap = new AnimationWrapper();

            var act1 = new MoveToEffect(enemy1, enemy2.Position, 400);
            var act2 = new MoveToEffect(enemy2, enemy1.Position, 400);

            wrap.Add(act1).Add(act2);
            _animationManager.Add(wrap);
            wrap.Waite();
        }

        private void SwapSelected(IEnemy enemy1, IEnemy enemy2)
        {
            if (!_gameField.IsNear(enemy1, enemy2)) return;

            SwapAnimation(enemy1, enemy2);
            _gameField.SwapEnemies(enemy1, enemy2);

            if (_gameField.Score == 0) {
                SwapAnimation(enemy1, enemy2);
                _gameField.SwapEnemies(enemy2, enemy1);
            }
        }

        void SwapEnemies(GameInputState state)
        {
            TryToSelect(state, _gameField);

            if (_firstSelEnemy != null && _secondSelEnemy != null) {
                _firstSelEnemy.Selected = false;
                SwapSelected(_firstSelEnemy, _secondSelEnemy);
                _firstSelEnemy = _secondSelEnemy = null;
            }
        }

        public void HandleUpdate(GameInputState state)
        {
            if (_settings.State == GameState.Play) {
                SwapEnemies(state);
            }
        }
    }
}
