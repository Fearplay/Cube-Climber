using System.Windows;
using System.Windows.Controls;

namespace CubeClimber.classes
{
    public class StartScreenClass
    {

        private readonly Button startGameButton;
        private readonly Button infoButton;
        private readonly Button quitGameButton;
        private readonly Button backButton;
        private readonly Label aboutGameLabel;
        private bool gamePause = true;

        public bool GamePause { get => gamePause; set => gamePause = value; }

        public StartScreenClass(MainWindow mainWindow)
        {
            startGameButton = mainWindow.startGameButton;
            infoButton = mainWindow.infoButton;
            quitGameButton = mainWindow.quitGameButton;
            backButton = mainWindow.backButton;
            aboutGameLabel = mainWindow.aboutGameLabel;

            startGameButton.Click += StartButtonClick;
            infoButton.Click += InfoButtonClick;
            quitGameButton.Click += QuitButtonClick;
            backButton.Click += BackButtonClick;

        }
        private void InfoScreen()
        {
            startGameButton.Visibility = Visibility.Hidden;
            infoButton.Visibility = Visibility.Hidden;
            quitGameButton.Visibility = Visibility.Hidden;
            backButton.Visibility = Visibility.Visible;
            aboutGameLabel.Visibility = Visibility.Visible;
        }
        private void MainMenuScreen()
        {
            startGameButton.Visibility = Visibility.Visible;
            infoButton.Visibility = Visibility.Visible;
            quitGameButton.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Hidden;
            aboutGameLabel.Visibility = Visibility.Hidden;
        }
        private void StartButtonClick(object sender, RoutedEventArgs e)
        {

            gamePause = false;
            startGameButton.Visibility = Visibility.Hidden;
            infoButton.Visibility = Visibility.Hidden;
            quitGameButton.Visibility = Visibility.Hidden;
        }
        private void QuitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void InfoButtonClick(object sender, RoutedEventArgs e)
        {
            InfoScreen();
            aboutGameLabel.Visibility = Visibility.Visible;


        }
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            MainMenuScreen();
        }
    }
}
