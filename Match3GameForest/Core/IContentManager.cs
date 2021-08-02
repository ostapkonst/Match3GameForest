namespace Match3GameForest.Core
{
    public interface IContentManager
    {
        ITexture2D LoadTexture(string assetName);
        ISoundEffect LoadSound(string assetName);
        T Get<T>(string assetName);
        void Set<T>(string assetName, T content);
    }
}
