using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Match3GameForest.Config;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Match3GameForest
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>

    public sealed partial class MainMenu : Page
    {
        private readonly int minRange = 6;
        private readonly int maxRange = 12;
        private readonly int defaultValue = 8;

        private readonly IList<int> MatrixRange;

        public MainMenu()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            MatrixRange = new List<int>();

            for (int i = minRange; i <= maxRange; i++) {
                MatrixRange.Add(i);
            }
        }

        private void StartMatch3()
        {
            var pi = new GameSettings()
            {
                MatrixRows = (int)rowsList.SelectedValue,
                MatrixColumns = (int)colsList.SelectedValue,
                PlayingDuration = (timeList.SelectedIndex + 1) * 30,
                PlaySound = soundControl.IsOn
            };

            Frame.Navigate(typeof(GameScreen), pi);
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            StartMatch3();
        }
    }
}
