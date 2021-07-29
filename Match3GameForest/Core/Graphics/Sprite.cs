using System;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Core
{
    public abstract class Sprite : ISprite
    {
        public Color Lightning { get; set; }
        public float Scale { get; set; }
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

        public void Update(GameInputState state)
        {
            if (Paused) return;

            BeforeUpdate?.Invoke(state);

            _sourceRect = new Rectangle(0, 0, FrameWidth, FrameHeight);

            AfterUpdate?.Invoke(state);
        }

        public void Draw(GameTime gameTime)
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

        public Vector2 Position { get; set; }

        public int ScaledWidth => (int)(FrameWidth * Scale);

        public int ScaledHeight => (int)(FrameHeight * Scale);

        private int FrameWidth => _spriteStrip.Width;

        private int FrameHeight => _spriteStrip.Height;

        public abstract Rectangle GetBounds();

        protected event Action<GameInputState> BeforeUpdate;
        protected event Action<GameInputState> AfterUpdate;
        protected event Action<GameTime> BeforeDraw;
        protected event Action<GameTime> AfterDraw;
    }
}
