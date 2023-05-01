using CubeClimber.classes.managementClasses;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CubeClimber.classes.logicClasses
{
    public class GameLogicClass
    {
        private readonly Color[] labelColors = new Color[] { Color.FromRgb(0, 180, 216), Color.FromRgb(238, 97, 35), Color.FromRgb(217, 4, 41), Color.FromRgb(255, 89, 94), Color.FromRgb(244, 213, 141) };
        private readonly Color[] canvasColors = new Color[] { Color.FromRgb(43, 45, 66), Color.FromRgb(0, 20, 39) };
        
        private readonly Image soundOnImage;
        private readonly Image soundOffImage;
        private readonly Canvas myCanvas;
        private readonly Label scoreLabel;
        private readonly Label livesLabel;
        private readonly Label highScoreLabel;
        private readonly Label gameOverLabel;
        private readonly Label restartGameLabel;
        private readonly Label quitGameLabel;

        private readonly Rectangle finishLane;
        private readonly MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

 
        private readonly StartScreenClass startScreenClass;
        private readonly SoundLogicClass soundLogic;
        private readonly PlayerManagementClass playerClass;
        private readonly WorkWithFilesClass workWithFiles = new();
        private readonly EnemyManagementClass enemyClass;
        private readonly Random random = new();
        private readonly DispatcherTimer gameTimer = new();
        public GameLogicClass(Canvas myCanvas)
        {
            this.myCanvas = myCanvas;
            scoreLabel = mainWindow.scoreLabel;
            highScoreLabel = mainWindow.highScoreLabel;
            livesLabel = mainWindow.livesLabel;

            gameOverLabel = mainWindow.gameOverLabel;
            restartGameLabel = mainWindow.restartGameLabel;
            quitGameLabel = mainWindow.quitGameLabel;

            finishLane = mainWindow.finishLane;


            soundOnImage = mainWindow.soundOnImage;
            soundOffImage = mainWindow.soundOffImage;

            startScreenClass = new StartScreenClass(mainWindow);
            soundLogic = new SoundLogicClass(soundOnImage, soundOffImage, mainWindow);
            playerClass = new PlayerManagementClass(mainWindow, myCanvas);
            enemyClass = new EnemyManagementClass(mainWindow, myCanvas, playerClass, soundLogic);
            mainWindow.Closing += WindowClosing;
        }
        public void OnStartMainWindow()
        {

            myCanvas.Focus();
            gameTimer.Tick += GameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
            OnStartGameLogic();
        }
        public void OnStartGameLogic()
        {
            playerClass.OnStartPlayer();
            SolidColorBrush labelRandomBackgroundColor = new(labelColors[random.Next(labelColors.Length)]);
            SolidColorBrush canvasRandomBackgroundColor = new(canvasColors[random.Next(canvasColors.Length)]);

            myCanvas.Background = canvasRandomBackgroundColor;
            scoreLabel.Background = labelRandomBackgroundColor;
            highScoreLabel.Background = labelRandomBackgroundColor;
            livesLabel.Background = labelRandomBackgroundColor;
            gameOverLabel.Background = labelRandomBackgroundColor;
            restartGameLabel.Background = labelRandomBackgroundColor;
            quitGameLabel.Background = labelRandomBackgroundColor;


            Panel.SetZIndex(gameOverLabel, 2);
            Panel.SetZIndex(restartGameLabel, 2);
            Panel.SetZIndex(quitGameLabel, 2);
        }

        private void ShowTexts()
        {

            scoreLabel.Content = "Score: " + workWithFiles.GetScore();
            highScoreLabel.Content = "High Score: " + workWithFiles.GetHighScore();
            livesLabel.Content = "Lives: " + playerClass.PlayerLives;

        }
        private void GameTimerEvent(object? sender, EventArgs e)
        {
            ShowTexts();

            if (enemyClass.IsLostGame == false && startScreenClass.GamePause == false)
            {

                enemyClass.MoveEnemy();

                playerClass.MovePlayer();

                HitFinishLane();

                enemyClass.SpawnEnemy();
            }
            else
            {
                if (startScreenClass.GamePause == false)
                {
                    GameOver();
                }


            }
        }

        private void HitFinishLane()
        {
            foreach (var x in myCanvas.Children.OfType<Rectangle>().ToList())
            {

                if ((string)x.Tag == "finishTag")
                {

                    Rect finishHitBox = new(0, 0, finishLane.Width, finishLane.Height);

                    soundLogic.StopNextRoundSound();
                    if (finishHitBox.IntersectsWith(playerClass.GetPlayerHitBox()))
                    {
                        soundLogic.PlayNextRoundSound();

                        workWithFiles.UpdateScore();

                        if (enemyClass.EnemySpeed < EnemyManagementClass.GetEndEnemySpeed() && workWithFiles.GetScore() % 5 == 0)
                        {

                            enemyClass.UpdateEnemySpeed();

                        }

                        playerClass.StartPlayer();

                    }
                    else
                    {
                        LevelLogic();
                    }
                }
            }
        }

        private void LevelLogic()
        {
            if (workWithFiles.GetScore() <= 15)
            {
                playerClass.IsLevelHarder = false;
            }
            else if (workWithFiles.GetScore() > 15 && workWithFiles.GetScore() < 30)
            {
                playerClass.IsLevelHarder = true;
                enemyClass.UpdateFastSpawn(2);
            }
            else
            {
                playerClass.IsLevelHarder = true;
                enemyClass.UpdateFastSpawn(3);
            }
        }

        private void GameOver()
        {

            gameOverLabel.Visibility = Visibility.Visible;
            restartGameLabel.Visibility = Visibility.Visible;
            quitGameLabel.Visibility = Visibility.Visible;

            if (Keyboard.IsKeyDown(Key.Return))
            {
                ResetGame();
            }
            else if (Keyboard.IsKeyDown(Key.Escape))
            {
                workWithFiles.UpdateHighScore();
                Application.Current.Shutdown();
            }
        }
        private void ResetGame()
        {
            gameOverLabel.Visibility = Visibility.Hidden;
            restartGameLabel.Visibility = Visibility.Hidden;
            quitGameLabel.Visibility = Visibility.Hidden;

            workWithFiles.UpdateHighScore();

            workWithFiles.SetScore();
            playerClass.UpdateLivesAfterDeath();

            enemyClass.ResetGame();
        }

        private void WindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            workWithFiles.UpdateHighScore();
        }

    }
}
