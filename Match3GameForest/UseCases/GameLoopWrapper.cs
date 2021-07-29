using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Match3GameForest.Config;
using Match3GameForest.Core;

namespace Match3GameForest.UseCases
{
    public class GameLoopWrapper : GameLoop, IRegistering
    {
        public GameLoopWrapper(IContentManager contentManager)
        {
            Next = new GenerateField(contentManager);
        }
    }
}
