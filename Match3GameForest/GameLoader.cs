using Autofac;
using Match3GameForest.Core;
using Match3GameForest.Entities;
using Match3GameForest.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Match3GameForest.UseCases;
using System.Threading.Tasks;

namespace Match3GameForest
{
    public class GameLoader : Game
    {
        private IContainer _container;
        private ISpriteBatch _spriteBatch;
        private GameLoopWrapper _gameLoop;
        private IAnimation _animateManager;
        public GameSettings GameData { get; private set; }
        private IGameField _gameField;
        private IBonusFactory _bonusFactory;
        private ITimer _timer;
        private IScreen _screen;
        private readonly GraphicsDeviceManager _graphics;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        private ISoundEffect _soundEffect;

        public event Action<GameSettings> OnUpdate;
        public event Action<GameSettings> OnDraw;

        public event Action OnStart;
        public event Action OnStop;

        public GameLoader()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _container = DIConfig.Register(this);

            _spriteBatch = _container.Resolve<ISpriteBatch>();
            _animateManager = _container.Resolve<IAnimation>();
            GameData = _container.Resolve<GameSettings>();
            _timer = _container.Resolve<ITimer>();
            _gameField = _container.Resolve<IGameField>();
            _bonusFactory = _container.Resolve<IBonusFactory>();
            _screen = _container.Resolve<IScreen>();

            var cm = PrepareCM(_container);
            _soundEffect = PrepareSound(cm);

            _gameLoop = new GameLoopWrapper(cm);

            _timer.OnFinish += StopGame;
        }

        private ISoundEffect PrepareSound(IContentManager cm)
        {
            var sound = cm.LoadSound("BackgroundSound");

            sound.IsLooped = true;
            sound.Volume = 0.8f;

            return sound;
        }

        private IContentManager PrepareCM(IContainer _container)
        {
            var cm = _container.Resolve<IContentManager>();

            cm.Set("animation", _animateManager);
            cm.Set("field", _gameField);
            cm.Set("bonuses", _bonusFactory);
            cm.Set("settings", GameData);
            cm.Set("timer", _timer);

            return cm;
        }

        public void StartGame(GameSettings pi)
        {
            _gameLoop.Dispose();       // Порядок запуска 
            _animateManager.Dispose(); // важен

            GameData.MatrixColumns = pi.MatrixColumns;
            GameData.MatrixRows = pi.MatrixRows;
            GameData.PlayingDuration = pi.PlayingDuration;
            GameData.PlaySound = pi.PlaySound;
            if (GameData.PlaySound) {
                _soundEffect.Play();
            }

            GameData.State = GameState.Init;

            OnStart?.Invoke();
        }

        public void StopGame()
        {
            if (GameData.PlaySound) {
                _soundEffect.Stop();
            }

            GameData.State = GameState.Finish;

            OnStop?.Invoke();
        }

        private GameInputState UpdateInputStates(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            var viewPort = GraphicsDevice.Viewport;

            var viewSize = new Point(viewPort.Width, viewPort.Height - 75); // Hotfix, MonoGame не учитывает XAML разметку

            return new GameInputState(
                _currentMouseState,
                _previousMouseState,
                gameTime,
                _animateManager.IsAnimate,
                viewSize,
                _screen.WorldToLocalScale());
        }

        protected override void Update(GameTime gameTime)
        {
            var currentGameState = UpdateInputStates(gameTime);

            _animateManager.Update(currentGameState);

            if (GameData.State != GameState.Finish) {
                _gameLoop.Update(currentGameState);
            }

            if (GameData.State == GameState.Play) {
                _screen.Update(currentGameState, _gameField);
                _gameField.Update(currentGameState);
                OnUpdate?.Invoke(GameData);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            if (GameData.State == GameState.Play) {
                _spriteBatch.Begin(_screen);
                _gameField.Draw(gameTime);
                OnDraw?.Invoke(GameData);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
