using System;

namespace Match3GameForest.Core
{
    public interface IContentManager
    {
        ITexture2D Load(string assetName);
        Lazy<T> Get<T>(string assetName);
        void Set<T>(string assetName, T content);
    }
}
