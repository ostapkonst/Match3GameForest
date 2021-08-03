using System;
using System.Collections.Generic;
using Match3GameForest.Config;
using Match3GameForest.Core;

namespace Match3GameForest.Entities
{
    public class EnemyFactory : IEnemyFactory, IRegistering
    {
        private readonly Random _random;
        private readonly ISpriteBatch _spriteBatch;
        private readonly IList<ITexture2D> _textures;
        private readonly List<string> _enemies;

        public float Scale { get; set; } = 0.5f;

        public EnemyFactory(IContentManager contentManager, ISpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _textures = new List<ITexture2D>();
            _random = new Random();
            _enemies = new List<string>();

            var _list = new string[] {
                "BlueDrop",
                "GreenLeaf",
                "OrangeSun",
                "RedHeart",
                "YellowStar",
            };

            _enemies.AddRange(_list);

            foreach (var enemy in _enemies) {
                _textures.Add(contentManager.LoadTexture(enemy));
            }
        }

        public IEnemy Build()
        {
            var tileNumber = _random.Next(_enemies.Count);
            return new Enemy(_spriteBatch, _textures[tileNumber], _enemies[tileNumber]) { Scale = Scale };
        }
    }
}