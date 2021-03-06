using System;
using Match3GameForest.Core;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Entities
{
    public class Enemy : Sprite, IEnemy
    {
        private Color _prevCollor;
        private bool _isSelected;

        public Enemy(ISpriteBatch spriteBatch, ITexture2D spriteStrip, string enemyType)
            : base(spriteBatch, spriteStrip)
        {
            Type = enemyType;
            IsActive = true;
            _isSelected = false;
            TouchedTime = DateTime.Now;
        }

        public bool IsActive { get; private set; }

        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)Position.X - ScaledWidth / 2,
                (int)Position.Y - ScaledHeight / 2,
                ScaledWidth,
                ScaledHeight);
        }

        public int Prize => 1;

        private MatrixPos _matrixPos;

        public string Type { get; private set; }

        public MatrixPos GetMatrixPos
        {
            get => _matrixPos;
            set {
                _matrixPos = value;
                TouchedTime = DateTime.Now;
            }
        }

        public bool Selected
        {
            get => _isSelected;
            set {
                if (value == _isSelected) return;

                if (value) {
                    _prevCollor = Lightning;
                    Lightning = Color.RoyalBlue;
                } else {
                    Lightning = _prevCollor;
                }

                _isSelected = value;
            }
        }

        public bool Collide(ICollided p2)
        {
            var r1 = GetBounds();
            var r2 = p2.GetBounds();

            return r1.Intersects(r2);
        }

        public DateTime TouchedTime { get; private set; }

        public event Action OnDestroy;

        public void Destroy()
        {
            IsActive = false;
            Hidden = true;
            OnDestroy?.Invoke();
        }
    }
}