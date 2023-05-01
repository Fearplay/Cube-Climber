using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CubeClimber.classes.managementClasses
{
    internal class PlayerManagementClass
    {
        private const int PlayerSpeed = 5;
        private const int StartLives = 3;
        
        private readonly Color[] playerColors = new Color[] { Color.FromRgb(0, 255, 0), Color.FromRgb(234, 140, 85), Color.FromRgb(255, 255, 255), Color.FromRgb(205, 180, 219) };
        
        private readonly MainWindow mainWindow;
        private readonly Canvas myCanvas;
        
        private readonly int playerSpeed = PlayerSpeed;
        private bool isLevelHarder;
        private int playerLives = StartLives;
        private readonly Random random = new();

        public int PlayerLives { get => playerLives; set => playerLives = value; }
        public bool IsLevelHarder { get => isLevelHarder; set => isLevelHarder = value; }

        public PlayerManagementClass(MainWindow mainWindow, Canvas myCanvas)
        {
            this.mainWindow = mainWindow;
            this.myCanvas = myCanvas;
        }
        public void OnStartPlayer()
        {
            StartPlayer();
            SolidColorBrush playerRandomColor = new(playerColors[random.Next(playerColors.Length)]);
            mainWindow.player.Fill = playerRandomColor;

        }
        public void StartPlayer()
        {
            mainWindow.player.Height = 25;
            mainWindow.player.Width = 25;

            double x = Application.Current.MainWindow.Width / 2;

            Canvas.SetLeft(mainWindow.player, x);
            Canvas.SetTop(mainWindow.player, 537);

        }
        public void MovePlayer()
        {
            if ((Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S)) && Canvas.GetTop(mainWindow.player) < myCanvas.ActualHeight - mainWindow.player.Height)
            {
                Canvas.SetTop(mainWindow.player, Canvas.GetTop(mainWindow.player) + playerSpeed);
            }
            if ((Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W)) && Canvas.GetTop(mainWindow.player) > 0)
            {
                Canvas.SetTop(mainWindow.player, Canvas.GetTop(mainWindow.player) - playerSpeed);
            }
            if ((Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A)) && Canvas.GetLeft(mainWindow.player) > 0 && IsLevelHarder == false)
            {
                Canvas.SetLeft(mainWindow.player, Canvas.GetLeft(mainWindow.player) - playerSpeed);
            }
            if ((Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D)) && Canvas.GetLeft(mainWindow.player) < myCanvas.ActualWidth - mainWindow.player.Width && IsLevelHarder == false)
            {
                Canvas.SetLeft(mainWindow.player, Canvas.GetLeft(mainWindow.player) + playerSpeed);
            }

        }
        public int UpdateLivesAfterHitEnemy()
        {
            int updatedLives = PlayerLives - 1;
            PlayerLives = updatedLives;

            return updatedLives;
        }

        public int UpdateLivesAfterDeath()
        {
            return playerLives = StartLives;
        }
        public Rect GetPlayerHitBox()
        {
            return new Rect(Canvas.GetLeft(mainWindow.player), Canvas.GetTop(mainWindow.player), mainWindow.player.Width, mainWindow.player.Height);
        }

    }
}
