using CubeClimber.classes.logicClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CubeClimber.classes.managementClasses
{
    internal class EnemyManagementClass
    {
        private const int StartEnemySpeed = 2;
        private const int EndEnemySpeed = 9;

        private readonly Color[] enemyColors = new Color[] { Color.FromRgb(242, 95, 92), Color.FromRgb(255, 0, 0), Color.FromRgb(255, 255, 0), Color.FromRgb(0, 255, 255), Color.FromRgb(255, 0, 255), Color.FromRgb(51, 92, 103), Color.FromRgb(242, 84, 45), Color.FromRgb(2, 195, 154) };
        private readonly List<Rectangle> items = new();

        private readonly MainWindow mainWindow;
        private readonly Canvas myCanvas;

        private int enemySpeed = StartEnemySpeed;
        private int spawnTime = 35;
        private int fastSpawn = 1;
        private bool isLostGame;

        private readonly SoundLogicClass soundLogic;
        private readonly PlayerManagementClass playerClass;

        private readonly Random random = new();

        public int EnemySpeed { get => enemySpeed; set => enemySpeed = value; }
        public int FastSpawn { get => fastSpawn; set => fastSpawn = value; }
        public bool IsLostGame { get => isLostGame; set => isLostGame = value; }

        public EnemyManagementClass(MainWindow mainWindow, Canvas myCanvas, PlayerManagementClass playerClass, SoundLogicClass soundLogic)
        {
            this.mainWindow = mainWindow;
            this.myCanvas = myCanvas;

            this.playerClass = playerClass;
            this.soundLogic = soundLogic;

        }
        private void MakeEnemy()
        {
            SolidColorBrush enemyRandomColor = new(enemyColors[random.Next(enemyColors.Length)]);

            Rectangle rectangle = new()
            {
                Width = 50,
                Height = 50,
                Fill = enemyRandomColor,
            };

            int x = 920;
            int y = random.Next(100 + (int)mainWindow.player.Height, 480 - (int)mainWindow.player.Height);

            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);

            items.Add(rectangle);
            myCanvas.Children.Add(rectangle);
        }
        public void SpawnEnemy()
        {
            spawnTime -= 1;
            if (spawnTime < 1)
            {
                MakeEnemy();
                if (FastSpawn == 1)
                {
                    spawnTime = 35;
                }
                else if (FastSpawn == 2)
                {
                    spawnTime = 20;
                }
                else
                {
                    spawnTime = 10;
                }
            }
        }

        public void MoveEnemy()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                Rectangle rectangle = items[i];
                Rect enemyHitBox = new(Canvas.GetLeft(rectangle), Canvas.GetTop(rectangle), rectangle.Width, rectangle.Height);

                Canvas.SetLeft(rectangle, Canvas.GetLeft(rectangle));

                if (enemyHitBox.IntersectsWith(playerClass.GetPlayerHitBox()))
                {
                    soundLogic.PlayHitSound();

                    playerClass.UpdateLivesAfterHitEnemy();

                    playerClass.StartPlayer();

                    if (playerClass.PlayerLives == 0)
                    {
                        IsLostGame = true;
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
        public void ResetGame()
        {
            IsLostGame = false;
            EnemySpeed = StartEnemySpeed;
            FastSpawn = 1;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                Rectangle rectangle = items[i];
                myCanvas.Children.Remove(rectangle);
                items.RemoveAt(i);
            }
        }

        public static int GetEndEnemySpeed()
        {
            return EndEnemySpeed;
        }

        public void UpdateFastSpawn(int fastSpawn)
        {
            FastSpawn = fastSpawn;
        }
        public int UpdateEnemySpeed()
        {
            int updatedSpeed = EnemySpeed + 1;

            EnemySpeed = updatedSpeed;
            return updatedSpeed;
        }
    }
}
