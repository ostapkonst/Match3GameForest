using System;
using System.Collections.Generic;
using Match3GameForest.Config;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Match3GameForest.Core
{
    public class ContentManagerWrapper : IContentManager, IRegistering
    {
        private readonly ContentManager _contentManager;
        private readonly IDictionary<string, object> _loaded;

        public ContentManagerWrapper(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _loaded = new Dictionary<string, object>();
        }

        public void Set<T>(string assetName, T content)
        {
            _loaded.Add(assetName, content);
        }

        public T Get<T>(string assetName)
        {
            try {
                return (T)_loaded[assetName];
            } catch {
                throw new ArgumentException($"Failed to get resource: {assetName}");
            }
        }

        public ITexture2D Load(string assetName)
        {
            var texture = _contentManager.Load<Texture2D>(assetName);
            return new Texture2DWrapper(texture);
        }
    }
}