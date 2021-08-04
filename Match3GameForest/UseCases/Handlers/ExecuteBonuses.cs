using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3GameForest.Config;
using Match3GameForest.Core;
using Match3GameForest.Entities;

namespace Match3GameForest.UseCases
{
    public class ExecuteBonuses : IGameLoop
    {
        private readonly IAnimation _animationManager;
        private readonly IGameField _gameField;
        private readonly GameSettings _settings;
        private readonly IBonusFactory _bonuses;

        public ExecuteBonuses(IContentManager contentManager)
        {
            _animationManager = contentManager.Get<IAnimation>("animation");
            _gameField = contentManager.Get<IGameField>("field");
            _settings = contentManager.Get<GameSettings>("settings");
            _bonuses = contentManager.Get<IBonusFactory>("bonuses");
        }

        private IAnimation DestroyAnimation(IList<IEnemy> series)
        {
            var wrap = new AnimationWrapper();
            foreach (var el in series) {
                wrap.Add(new RescaleEffect(el, 1f, 0.6f, 450));
            }
            _animationManager.Add(wrap);
            return wrap;
        }

        private void BombAnimation(IBonus bonus)
        {
            var act1 = new WaiteTime(250); // Задержка по ТЗ

            act1.Next += () =>
            {
                var field = _gameField.GetField();
                var enemies = new List<IEnemy>();

                var carrirer = bonus.Carrier;
                var carrierPos = carrirer.GetMatrixPos;
                var Col = carrierPos.X;
                var Row = carrierPos.Y;

                for (var row = Row - 1; row <= Row + 1; row++) {
                    for (var col = Col - 1; col <= Col + 1; col++) {
                        if (row < 0 || row >= _gameField.MatrixRows) continue;
                        if (col < 0 || col >= _gameField.MatrixColumns) continue;

                        var enemy = field.Series[row][col];

                        if (!enemy.IsActive) continue;

                        enemies.Add(enemy);
                    }
                }

                DestroyAnimation(enemies).Next += () =>
                {
                    foreach (var enemy in enemies) {
                        enemy.Destroy();
                        _settings.GameScore += enemy.Prize;
                    }
                };
            };

            _animationManager.Add(act1);
        }

        private void ActivateAll()
        {
            if (!_bonuses.IsActivate) return;

            var allActiveBonuses = _bonuses.GetActiveBonuses();
            foreach (var bonus in allActiveBonuses) {
                BombAnimation(bonus);
                bonus.IsActivate = false;
            }
        }

        public void HandleUpdate(GameInputState state)
        {
            if (_settings.State == GameState.Play) {
                ActivateAll();
            }
        }
    }
}
