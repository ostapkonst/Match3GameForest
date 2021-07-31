using Match3GameForest.Core;

namespace Match3GameForest.UseCases
{
    public class GameLoopWrapper : GameLoop
    {
        public GameLoopWrapper(IContentManager contentManager)
        {
            Next = new GenerateField(contentManager);
        }
    }
}
