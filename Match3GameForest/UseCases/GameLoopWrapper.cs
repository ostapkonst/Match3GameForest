using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.UseCases
{
    public class GameLoopWrapper : IGameLoop
    {
        private IDictionary<IGameLoop, Task> dictionaryUpdate;
        private IList<IGameLoop> handlers;

        public GameLoopWrapper(IContentManager contentManager)
        {
            dictionaryUpdate = new Dictionary<IGameLoop, Task>();

            handlers = new List<IGameLoop>() {
                new GenerateField(contentManager),
                new DestroyEnemies(contentManager),
                new DisplayField(contentManager),
                new SwapTwoEnemies(contentManager),
                new UpdateTimer(contentManager),
            };
        }

        private void CreateUpdateTask(IGameLoop key, GameInputState state)
        {
            if (dictionaryUpdate.TryGetValue(key, out Task task))
                if (task.IsCompleted)
                    dictionaryUpdate.Remove(key);
                else
                    return;

            var newTask = new Task(() => key.HandleUpdate(state));
            dictionaryUpdate.Add(key, newTask);
            newTask.Start();
        }

        public void HandleUpdate(GameInputState state)
        {
            foreach (var handel in handlers) {
                CreateUpdateTask(handel, state);
            }
        }
    }
}
