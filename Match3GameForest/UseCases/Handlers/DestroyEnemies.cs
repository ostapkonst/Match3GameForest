using System;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Match3GameForest.Entities;

namespace Match3GameForest.UseCases
{
    public class DestroyEnemies : GameLoop
    {
        private readonly IAnimation _animationManager;
        private readonly IGameField _gameField;
        private readonly GameSettings _settings;

        public DestroyEnemies(IContentManager contentManager)
        {
            _animationManager = contentManager.Get<IAnimation>("animation");
            _gameField = contentManager.Get<IGameField>("field");
            _settings = contentManager.Get<GameSettings>("settings");

            Next = new UpdateTimer(contentManager);
        }

        private void DestroyAll(GameSettings _settings, IGameField gameField, IAnimation animation)
        {
            var series = gameField.GetSeries();
            var score = series.Score;
            var wrap1 = new AnimationWrapper();

            if (score > 0) {
                foreach (var el in series) {
                    wrap1.Add(new RescaleEffect(el, 1f, 0.6f, 500));
                };
            }

            wrap1.AfterAnimate = () =>
            {
                var createdEnemies = gameField.Match(series);

                foreach (var el in createdEnemies) {
                    wrap1.Add(new RescaleEffect(el, 0.6f, 1f, 500));
                };

                _settings.GameScore += score;
            };

            animation.Add(wrap1);
        }

        public override void HandleUpdate(GameInputState state)
        {
            if (!_animationManager.IsAnimate) {
                DestroyAll(_settings, _gameField, _animationManager);
            }

            base.HandleUpdate(state);
        }
    }
}
