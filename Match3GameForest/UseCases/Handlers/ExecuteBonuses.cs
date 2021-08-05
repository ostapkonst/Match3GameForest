using System;
using System.Collections.Generic;
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

        private void DestroyAnimation(IAnimation wrap, IEnemy enemy)
        {
            enemy.Hidden = false;
            var anim1 = new RescaleEffect(enemy, 1f, 0.6f, 100);
            anim1.Next += () =>
            {
                enemy.Hidden = true;
                _settings.GameScore += enemy.Prize;
            };
            wrap.Add(anim1);
        }

        private void DestroyAnimation(IList<IEnemy> series)
        {
            var wrap = new AnimationWrapper();
            foreach (var enemy in series) {
                DestroyAnimation(wrap, enemy);
            }
            _animationManager.Add(wrap);
        }

        private void BombAnimation(BombBonus bomb)
        {
            var act1 = new WaiteEffect(250); // Задержка по ТЗ

            act1.Next += () =>
            {
                var field = _gameField.GetField();
                var enemies = new List<IEnemy>();

                var carrirer = bomb.Carrier;
                var carrierPos = carrirer.GetMatrixPos;
                var Col = carrierPos.X;
                var Row = carrierPos.Y;

                for (var row = Row - 1; row <= Row + 1; row++) {
                    for (var col = Col - 1; col <= Col + 1; col++) {
                        if (row < 0 || row >= _gameField.MatrixRows) continue;
                        if (col < 0 || col >= _gameField.MatrixColumns) continue;

                        var enemy = field.Series[row][col];

                        if (!enemy.IsActive) continue;

                        enemy.Destroy();

                        enemies.Add(enemy);
                    }
                }

                DestroyAnimation(enemies);
            };

            _animationManager.Add(act1);
        }

        private void RocketFlyAnimation(Destroyer destr, IAnimation wrap)
        {
            var act1 = new MoveDirectionEffect(destr, 350, _gameField);

            act1.Next += () =>
            {
                destr.Destroy();
            };

            act1.Parallel += () =>
            {
                var field = _gameField.GetField();

                // TODO: Оптимизировать проверку столкновений
                foreach (var enemy in field) {
                    if (!enemy.IsActive) continue;
                    if (!enemy.Collide(destr)) continue;

                    enemy.Destroy();
                    DestroyAnimation(_animationManager, enemy);
                }
            };

            wrap.Add(act1);
        }

        private void LineAnimation(LineBonus line)
        {
            var d1 = line.GetDestroyer();
            var d2 = line.GetDestroyer(false);

            var wrap = new AnimationWrapper();

            RocketFlyAnimation(d1, wrap);
            RocketFlyAnimation(d2, wrap);

            _animationManager.Add(wrap);
        }

        private void ActivateAll()
        {
            if (!_bonuses.IsActivate) return;

            var allActiveBonuses = _bonuses.GetActiveBonuses();
            foreach (var bonus in allActiveBonuses) {
                switch (bonus) { // TODO: Вынести в методы классов
                    case BombBonus bomb:
                        BombAnimation(bomb);
                        break;
                    case LineBonus line:
                        LineAnimation(line);
                        break;
                    default:
                        throw new NotImplementedException("Bonus handler not implemented");
                }
                bonus.Deactivate();
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
