using System;
using System.Collections.Generic;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Match3GameForest.Entities;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public class GenerateField : IGameLoop
    {
        private readonly GameSettings _settings;
        private readonly IAnimation _animationManager;
        private readonly IGameField _gameField;

        public GenerateField(IContentManager contentManager)
        {
            _animationManager = contentManager.Get<IAnimation>("animation");
            _gameField = contentManager.Get<IGameField>("field");
            _settings = contentManager.Get<GameSettings>("settings");

            _gameField.OnCreate += CreateAnimation;
            _gameField.OnMove += MoveAnimation;
        }

        private void CreateAnimation(IList<IEnemy> series)
        {
            var wrap = new AnimationWrapper();
            foreach (var el in series) {
                wrap.Add(new RescaleEffect(el, 0.6f, 1f, 450));
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

        public void HandleUpdate(GameInputState state)
        {
            if (_settings.State == GameState.Init) {
                _gameField.GenerateField(_settings.MatrixRows, _settings.MatrixColumns);
                _settings.GameScore = 0;
                _settings.State = GameState.Timed;
            }

            if (_settings.State == GameState.Play) {
                if (_animationManager.IsAnimate) return;
                _gameField.RefreshField();
            }
        }
    }
}
