using System;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public abstract class Sprite : ISprite
    {
        public Color Lightning
        {
            get => _lightning;
            set {
                _lightning = value;
                OnLightning?.Invoke(value);
            }
        }

        public Guid Id { get; private set; }

        private readonly ISpriteBatch _spriteBatch;
        private readonly ITexture2D _spriteStrip;
        private Rectangle _sourceRect;

        protected Sprite(ISpriteBatch spriteBatch, ITexture2D spriteStrip)
        {
            Id = Guid.NewGuid();
            _spriteStrip = spriteStrip;
            Scale = 1f;
            Lightning = Color.White;
            _spriteBatch = spriteBatch;
            Paused = false;
            Hidden = false;
            Position = Vector2.Zero;
        }

        public bool Paused { get; set; }
        public bool Hidden { get; set; }

        public virtual void Update(GameInputState state)
        {
            if (Paused) return;

            BeforeUpdate?.Invoke(state);

            _sourceRect = new Rectangle(0, 0, FrameWidth, FrameHeight);

            AfterUpdate?.Invoke(state);
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (Hidden) return;

            BeforeDraw?.Invoke(gameTime);

            var destRect = new Rectangle(
                (int)Position.X - ScaledWidth / 2,
                (int)Position.Y - ScaledHeight / 2,
                ScaledWidth,
                ScaledHeight);

            _spriteBatch.Draw(_spriteStrip, destRect, _sourceRect, Lightning);

            AfterDraw?.Invoke(gameTime);
        }

        private Vector2 _position;

        public Vector2 Position
        {
            get => _position;
            set {
                _position = value;
                OnPosition?.Invoke(value);
            }
        }

        private float _scale;
        private Color _lightning;

        public float Scale
        {
            get => _scale;
            set {
                _scale = value;
                OnScale?.Invoke(_scale);
            }
        }

        public int ScaledWidth => (int)(FrameWidth * Scale);

        public int ScaledHeight => (int)(FrameHeight * Scale);

        private int FrameWidth => _spriteStrip.Width;

        private int FrameHeight => _spriteStrip.Height;

        public object Clone()
        {
            var sprite = (Sprite)MemberwiseClone();
            sprite.Id = Guid.NewGuid();
            return sprite;
        }

        public event Action<GameInputState> BeforeUpdate;
        public event Action<GameInputState> AfterUpdate;
        public event Action<GameTime> BeforeDraw;
        public event Action<GameTime> AfterDraw;
        public event Action<float> OnScale;
        public event Action<Vector2> OnPosition;
        public event Action<Color> OnLightning;
    }
}
