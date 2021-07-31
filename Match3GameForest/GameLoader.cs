using Autofac;
using Match3GameForest.Core;
using Match3GameForest.Entities;
using Match3GameForest.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Match3GameForest.UseCases;

namespace Match3GameForest
{
    public class GameLoader : Game
    {
        private IContainer _container;
        private ISpriteBatch _spriteBatch;
        private IGameLoop _gameLoop;
        private IAnimation _animateManager;
        private GameSettings _gameData;
        private IGameField _gameField;
        private ITimer _timer;
        private IScreen _screen;
        private readonly GraphicsDeviceManager _graphics;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;

        public event Action<GameSettings> OnUpdate;
        public event Action<GameSettings> OnDraw;
        public event Action OnExit;

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
            _gameData = _container.Resolve<GameSettings>();
            _timer = _container.Resolve<ITimer>();
            _gameField = _container.Resolve<IGameField>();
            _screen = _container.Resolve<IScreen>();

            _gameLoop = new GameLoopWrapper(PrepareCM(_container));
            _timer.OnFinish += GameExitHandler;
            StartGame();
        }

        private IContentManager PrepareCM(IContainer _container)
        {
            var cm = _container.Resolve<IContentManager>();

            cm.Set("animation", _animateManager);
            cm.Set("field", _gameField);
            cm.Set("settings", _gameData);
            cm.Set("timer", _timer);

            return cm;
        }

        public void StartGame()
        {
            _gameData.State = GameState.Init;
        }

        private void GameExitHandler()
        {
            _gameData.State = GameState.Finish;
            OnExit?.Invoke();
        }

        private GameInputState UpdateInputStates(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            var viewPort = GraphicsDevice.Viewport;

            var viewSize = new Point(viewPort.Width, viewPort.Height - 60); // Hotfix, MonoGame не учитывает XAML разметку

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
            _gameLoop.HandleUpdate(currentGameState);
            _screen.Update(currentGameState, _gameField);
            OnUpdate?.Invoke(_gameData);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            _gameField.Draw(gameTime);
            OnDraw?.Invoke(_gameData);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
