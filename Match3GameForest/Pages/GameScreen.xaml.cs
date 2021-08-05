using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Match3GameForest.Config;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Match3GameForest
{
    public sealed partial class GameScreen : Page
    {
        readonly GameLoader _game;
        private readonly CoreDispatcher _currentWindows;

        public GameScreen()
        {
            this.InitializeComponent();

            // https://github.com/MonoGame/MonoGame/issues/6089
            this.NavigationCacheMode = NavigationCacheMode.Required;

            // Create the game.
            var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<GameLoader>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
            _currentWindows = CoreWindow.GetForCurrentThread().Dispatcher;

            _game.OnDraw += DisplayOnPage;
            _game.OnStop += NavigateToGameOver;
        }

        public void DisplayOnPage(GameSettings gameData)
        {
            Func<Task> func = () =>
            {
                textScore.Text = $"Score: {gameData.GameScore}";
                textTimeLeft.Text = $"Time left: {gameData.TimeLeft}";
                return Task.CompletedTask;
            };

            _ = _currentWindows.RunTaskAsync(func);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;

            var pi = (GameSettings)e.Parameter;
            _game.StartGame(pi);
        }

        public void NavigateToGameOver()
        {
            Func<Task> func = () =>
            {
                Frame.Navigate(typeof(GameOver), _game.GameData);
                return Task.CompletedTask;
            };

            _ = _currentWindows.RunTaskAsync(func);
        }

        private async void exitButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "Confirmation of action",
                Content = "Are you sure you want to complete the game?",
                PrimaryButtonText = "Exit",
                SecondaryButtonText = "Restart",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary
            };

            _game.PauseGame();

            var result = await dialog.ShowAsync();

            switch (result) {
                case ContentDialogResult.Primary:
                    _game.StopGame();
                    Frame.Navigate(typeof(GameOver), _game.GameData);
                    break;
                case ContentDialogResult.Secondary:
                    _game.RestartGame();
                    break;
                default:
                    _game.ResumeGame();
                    break;
            }
        }
    }
}
