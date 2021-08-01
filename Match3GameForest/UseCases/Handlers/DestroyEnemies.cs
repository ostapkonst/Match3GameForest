using System;
using System.Collections.Generic;
using System.Diagnostics;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Match3GameForest.Entities;
using Microsoft.Xna.Framework;

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
            _gameField.OnMove += MoveAnimation;
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

        private void MoveAnimation(IList<Tuple<IEnemy, Vector2>> series)
        {
            var wrap = new AnimationWrapper();
            foreach (var el in series) {
                wrap.Add(new MoveToEffect(el.Item1, el.Item2, 650));
            }
            _animationManager.Add(wrap);
            wrap.Waite();
        }

        private void DestroyAll(GameSettings _settings, IGameField gameField, IAnimation animation)
        {
            var score = gameField.Score;

            if (score == 0) return;

            gameField.Match();

            _settings.GameScore += score;
        }

        public void HandleUpdate(GameInputState state)
        {
            if (_settings.State != GameState.Play) return;

            DestroyAll(_settings, _gameField, _animationManager);
        }
    }
}
