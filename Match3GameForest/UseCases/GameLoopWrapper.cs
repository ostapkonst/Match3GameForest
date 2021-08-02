using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public class GameLoopWrapper : IDisposable 
    {
        private IDictionary<IGameLoop, Task> _dictionaryUpdate;
        private IList<IGameLoop> _handlers;
        private CancellationTokenSource _cts;
        private CancellationToken _ct;

        public GameLoopWrapper(IContentManager contentManager)
        {
            _dictionaryUpdate = new Dictionary<IGameLoop, Task>();

            GenerateToken();

            _handlers = new List<IGameLoop>() {
                new GenerateField(contentManager),
                new DestroyEnemies(contentManager),
                new DisplayField(contentManager),
                new SwapTwoEnemies(contentManager),
                new UpdateTimer(contentManager),
            };
        }

        private void GenerateToken()
        {
            _cts = new CancellationTokenSource();
            _ct = _cts.Token;
        }

        private void CreateUpdateTask(IGameLoop key, GameInputState state)
        {
            if (_dictionaryUpdate.TryGetValue(key, out Task task))
                if (task.Status != TaskStatus.Running)
                    _dictionaryUpdate.Remove(key);
                else
                    return;

            var newTask = Task.Factory.StartNew(() => key.HandleUpdate(state), _ct);
            _dictionaryUpdate.Add(key, newTask);
         }

        public void Update(GameInputState state)
        {
            foreach (var handle in _handlers) {
                CreateUpdateTask(handle, state);
            }
        }

        public void CancelTasks()
        {
            _cts.Cancel();
            GenerateToken();
        }

        public void Dispose()
        {
            CancelTasks();
        }
    }
}
