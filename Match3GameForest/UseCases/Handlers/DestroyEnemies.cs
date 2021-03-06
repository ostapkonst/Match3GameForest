using System.Collections.Generic;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Match3GameForest.Entities;

namespace Match3GameForest.UseCases
{
    public class DestroyEnemies : IGameLoop
    {
        private readonly IAnimation _animationManager;
        private readonly IGameField _gameField;
        private readonly GameSettings _settings;

        public DestroyEnemies(IContentManager contentManager)
        {
            _animationManager = contentManager.Get<IAnimation>("animation");
            _gameField = contentManager.Get<IGameField>("field");
            _settings = contentManager.Get<GameSettings>("settings");

            _gameField.OnDestroy += DestroyAnimation;
        }

        private void DestroyAnimation(IList<IEnemy> series)
        {
            var wrap = new AnimationWrapper();
            foreach (var el in series) {
                wrap.Add(new RescaleEffect(el, 1f, 0.6f, 450));
            }
            _animationManager.Add(wrap);
            wrap.Waite();
        }

        private void DestroyAll()
        {
            var score = _gameField.Score;

            if (score == 0) return;

            _gameField.Match();

            _settings.GameScore += score;
        }

        public void HandleUpdate(GameInputState state)
        {
            if (_settings.State == GameState.Play) {
                if (_animationManager.IsAnimate) return;
                DestroyAll();
            }
        }
    }
}
