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
    // Каждый Handler обрабатывается отдельным таском

    public class GameLoopWrapper : IDisposable
    {
        private IDictionary<IGameLoop, Task> _dictionaryUpdate;
        private IList<IGameLoop> _handlers;

        public GameLoopWrapper(IContentManager contentManager)
        {
            _dictionaryUpdate = new Dictionary<IGameLoop, Task>();

            _handlers = new List<IGameLoop>() {
                new GenerateField(contentManager),
                new SwapTwoEnemies(contentManager),
                new DestroyEnemies(contentManager),
                new ExecuteBonuses(contentManager),
                new UpdateTimer(contentManager),
            };
        }

        private void CreateUpdateTask(IGameLoop key, GameInputState state)
        {
            if (_dictionaryUpdate.TryGetValue(key, out Task task))
                if (task.Status != TaskStatus.Running)
                    _dictionaryUpdate.Remove(key);
                else
                    return;

            var newTask = Task.Factory.StartNew(() => key.HandleUpdate(state));
            _dictionaryUpdate.Add(key, newTask);
        }

        public void Update(GameInputState state)
        {
            foreach (var handle in _handlers) {
                CreateUpdateTask(handle, state);
            }
        }

        public void Dispose()
        {
            var allTasks = _dictionaryUpdate.Values.ToArray();
            Task.WaitAll(allTasks);
            _dictionaryUpdate.Clear();
        }
    }
}
