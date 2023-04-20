using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CubeClimber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {

        private const int StartScore = 0;
        private const int StartHighScore = 0;
        private const int StartLives = 3;
        private const int PlayerSpeed = 5;
        private const int StartEnemySpeed = 2;
        private const int EndEnemySpeed = 9;
        private const string NextRoundSound = @"sounds/next_round.wav";
        private const string HitSound = @"sounds/hit.wav";

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);



        Random random = new Random();
        List<Rectangle> items = new List<Rectangle>();
        DispatcherTimer gameTimer = new DispatcherTimer();
        Brush customColor;
        Color[] labelColors = new Color[] { Color.FromRgb(0, 180, 216), Color.FromRgb(238, 97, 35), Color.FromRgb(217, 4, 41), Color.FromRgb(255, 89, 94), Color.FromRgb(244, 213, 141) };
        Color[] canvasColors = new Color[] { Color.FromRgb(43, 45, 66), Color.FromRgb(0, 20, 39) };
        Color[] playerColors = new Color[] { Color.FromRgb(0, 255, 0), Color.FromRgb(234, 140, 85), Color.FromRgb(255, 255, 255), Color.FromRgb(205, 180, 219) };
        Color[] enemyColors = new Color[] { Color.FromRgb(242, 95, 92), Color.FromRgb(255, 0, 0), Color.FromRgb(255, 255, 0), Color.FromRgb(0, 255, 255), Color.FromRgb(255, 0, 255), Color.FromRgb(51, 92, 103), Color.FromRgb(242, 84, 45), Color.FromRgb(2, 195, 154) };

        MediaPlayer nextRoundMediaPlayer = new MediaPlayer();
        MediaPlayer hitMediaPlayer = new MediaPlayer();
        MediaPlayer backgroundMusicMediaPlayer = new MediaPlayer();

        string appDirectory;
        string filePath;
        bool isMKeyPressed = false;
        int playerLives = StartLives;
        int enemySpeed = StartEnemySpeed;
        int score = StartScore;
        bool isLevelHarder;
        bool isMusicMuted;
        int playerSpeed = PlayerSpeed;
        string gameMusic = "sounds/game_music{0}.wav";

        int spawnTime = 20;

        int highScore = StartHighScore;


        int fastSpawn = 1;
        bool gameBad = false;
        bool gamePause = true;




        public MainWindow()
        {

            InitializeComponent();
            myCanvas.Focus();

            gameTimer.Tick += GameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
            appDirectory = System.IO.Path.Combine(appDataPath, "CubeClimber");
            filePath = System.IO.Path.Combine(appDirectory, "high_score.txt");
            OnStart();




        }



        private void OnStart()
        {
            ReadHighScore();


            backgroundMusicMediaPlayer.Open(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format(gameMusic, random.Next(1, 4)))));

            backgroundMusicMediaPlayer.Volume = 1;
            backgroundMusicMediaPlayer.Play();
            backgroundMusicMediaPlayer.MediaEnded += new EventHandler(MediaEnded);
            PlayerStart();
            SolidColorBrush labelRandomBackgroundColor = new SolidColorBrush(labelColors[random.Next(labelColors.Length)]);
            SolidColorBrush canvasRandomBackgroundColor = new SolidColorBrush(canvasColors[random.Next(canvasColors.Length)]);
            SolidColorBrush playerRandomColor = new SolidColorBrush(playerColors[random.Next(playerColors.Length)]);
            myCanvas.Background = canvasRandomBackgroundColor;
            scoreLabel.Background = labelRandomBackgroundColor;
            highScoreLabel.Background = labelRandomBackgroundColor;
            livesLabel.Background = labelRandomBackgroundColor;
            gameOverLabel.Background = labelRandomBackgroundColor;
            restartGameLabel.Background = labelRandomBackgroundColor;
            quitGameLabel.Background = labelRandomBackgroundColor;
            player.Fill = playerRandomColor;
            Canvas.SetZIndex(gameOverLabel, 2);
            Canvas.SetZIndex(restartGameLabel, 2);
            Canvas.SetZIndex(quitGameLabel, 2);
        }
        private void MediaEnded(object sender, EventArgs e)
        {
            backgroundMusicMediaPlayer.Position = TimeSpan.Zero;
            backgroundMusicMediaPlayer.Play();
        }
        private void ShowTexts()
        {
            scoreLabel.Content = "Score: " + score;
            highScoreLabel.Content = "High Score: " + highScore;
            livesLabel.Content = "Lives: " + playerLives;

        }
        private void GameTimerEvent(object? sender, EventArgs e)
        {
            ShowTexts();

            if (gameBad == false && gamePause == false)
            {
                MoveEnemy();



                if ((Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S)) && Canvas.GetTop(player) < (myCanvas.ActualHeight - player.Height))
                {
                    Canvas.SetTop(player, Canvas.GetTop(player) + playerSpeed);
                }
                if ((Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W)) && Canvas.GetTop(player) > 0)
                {
                    Canvas.SetTop(player, Canvas.GetTop(player) - playerSpeed);
                }
                if ((Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A)) && Canvas.GetLeft(player) > 0 && isLevelHarder == false)
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
                }
                if ((Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D)) && Canvas.GetLeft(player) < (myCanvas.ActualWidth - player.Width) && isLevelHarder == false)
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
                }

                HitFinishLane();

                SpawnEnemy();
            }
            else
            {
                if (gamePause == false)
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

                    Rect finishHitBox = new Rect(0, 0, finishLane.Width, finishLane.Height);

                    Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
                    nextRoundMediaPlayer.Volume = 0;
                    nextRoundMediaPlayer.Open(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NextRoundSound)));


                    nextRoundMediaPlayer.Stop();
                    if (finishHitBox.IntersectsWith(playerHitBox))
                    {



                        nextRoundMediaPlayer.Volume = 0.3;
                        nextRoundMediaPlayer.Play();

                        score += 1;


                        if (score > 1)
                        {

                            if (enemySpeed < EndEnemySpeed && score % 5 == 0)
                            {

                                enemySpeed += 1;
                            }

                        }
                        PlayerStart();

                    }
                    else
                    {


                        LevelLogic();
                    }
                }
            }
        }


        private void ReadHighScore()
        {
            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
                WriteTextFile();
            }
            if (!File.Exists(filePath))
            {
                WriteTextFile();
            }
            else
            {
                TextReader tr = new StreamReader(filePath);

                highScore = Convert.ToInt32(tr.ReadLine());
                tr.Close();
            }
        }

        private void UpdateHighScore()
        {

            if (score > highScore)
            {
                highScore = score;
                WriteTextFile();
            }
        }

        private void WriteTextFile()
        {


            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(highScore);
                writer.Close();
            }






        }

        private void SpawnEnemy()
        {

            spawnTime -= 1;
            if (spawnTime < 1)
            {

                MakeEnemy();
                if (fastSpawn == 1)
                {
                    spawnTime = 35;
                }
                else if (fastSpawn == 2)
                {
                    spawnTime = 20;
                }
                else
                {
                    spawnTime = 10;
                }

            }
        }

        private void LevelLogic()
        {
            if (score <= 15)
            {

                isLevelHarder = false;

            }
            else if (score > 15 && score < 30)
            {

                isLevelHarder = true;
                fastSpawn = 2;

            }
            else
            {
                isLevelHarder = true;
                fastSpawn = 3;
            }


        }

        private void MoveEnemy()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                Rectangle rectangle = items[i];
                Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
                Rect enemyHitBox = new Rect(Canvas.GetLeft(rectangle), Canvas.GetTop(rectangle), rectangle.Width, rectangle.Height);


                Canvas.SetLeft(rectangle, Canvas.GetLeft(rectangle));


                if (enemyHitBox.IntersectsWith(playerHitBox))
                {
                    hitMediaPlayer.Open(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HitSound)));
                    hitMediaPlayer.Volume = 0.3;
                    hitMediaPlayer.Play();
                    PlayerStart();
                    playerLives -= 1;
                    if (playerLives == 0)
                    {
                        gameBad = true;
                    }


                }

                else
                {

                    Canvas.SetLeft(rectangle, Canvas.GetLeft(rectangle) - enemySpeed);
                    if (Canvas.GetLeft(rectangle) < -rectangle.Width)
                    {
                        myCanvas.Children.Remove(rectangle);
                        items.RemoveAt(i);
                    }
                }






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
                UpdateHighScore();
                Application.Current.Shutdown();
            }
        }
        private void ResetGame()
        {

            gameOverLabel.Visibility = Visibility.Hidden;
            restartGameLabel.Visibility = Visibility.Hidden;
            quitGameLabel.Visibility = Visibility.Hidden;
            UpdateHighScore();
            fastSpawn = 1;
            enemySpeed = StartEnemySpeed;
            score = StartScore;
            playerLives = StartLives;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                Rectangle rectangle = items[i];
                myCanvas.Children.Remove(rectangle);
                items.RemoveAt(i);
            }
            gameBad = false;
        }

        private void MakeEnemy()
        {
            customColor = new SolidColorBrush(Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255)));
            SolidColorBrush enemyRandomColor = new SolidColorBrush(enemyColors[random.Next(enemyColors.Length)]);

            Rectangle rectangle = new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = enemyRandomColor,
            };

            int x = 920;
            int y = random.Next(100 + (int)player.Height, 500 - (int)player.Height);

            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);

            items.Add(rectangle);
            myCanvas.Children.Add(rectangle);



        }



        private void PlayerStart()
        {

            player.Height = 25;
            player.Width = 25;

            double x = Application.Current.MainWindow.Width / 2;
            double y = Application.Current.MainWindow.Height;




            Canvas.SetLeft(player, x);
            Canvas.SetTop(player, 537);

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

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateHighScore();
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

        private async void MuteBackgroundMusic()
        {
            if (Keyboard.IsKeyDown(Key.M))
            {
                if (!isMKeyPressed)
                {
                    isMKeyPressed = true;
                    if (isMusicMuted == true)
                    {

                        soundOnImage.Visibility = Visibility.Visible;
                        soundOffImage.Visibility = Visibility.Hidden;
                        isMusicMuted = false;
                        backgroundMusicMediaPlayer.Play();

                    }
                    else
                    {

                        soundOnImage.Visibility = Visibility.Hidden;
                        soundOffImage.Visibility = Visibility.Visible;
                        isMusicMuted = true;
                        backgroundMusicMediaPlayer.Stop();

                    }
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    isMKeyPressed = false;
                }
            }
        }
        private void KeyDownMute(object sender, KeyEventArgs e)
        {
            MuteBackgroundMusic();
        }


    }
}
