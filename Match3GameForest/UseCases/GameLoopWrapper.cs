using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    // Все Handlers обрабатываются в одном таске
    // Это нужно для работы Animation.Waite в events

    public class GameLoopWrapper : IDisposable
    {
        private IList<IGameLoop> _handlers;
        private Task _currentTask;

        public GameLoopWrapper(IContentManager contentManager)
        {
            _currentTask = Task.Run(() => { });

            _handlers = new List<IGameLoop>() {
                new ExecuteBonuses(contentManager), // Предыдущая анимация могла добавить бонусов
                new GenerateField(contentManager),
                new SwapTwoEnemies(contentManager),
                new DestroyEnemies(contentManager),
                new UpdateTimer(contentManager),
            };
        }

        public void Update(GameInputState state)
        {
            if (_currentTask.Status == TaskStatus.Running) return;

            _currentTask = Task.Run(() =>
            {
                foreach (var handle in _handlers) {
                    handle.HandleUpdate(state);
                }
            });
        }

        public void Dispose()
        {
            Task.WaitAll(_currentTask);
        }
    }
}
