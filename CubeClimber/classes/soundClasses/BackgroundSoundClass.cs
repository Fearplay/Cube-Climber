using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CubeClimber.classes.soundScripts
{
    internal class BackgroundSoundClass
    {
        private readonly string gameMusic = "sounds/game_music{0}.wav";

        private readonly Image soundOnImage;
        private readonly Image soundOffImage;
        private bool isMusicMuted;
        private bool isMKeyPressed = false;

        private readonly MediaPlayer backgroundMusicMediaPlayer = new();
        private readonly Random random = new();

        public BackgroundSoundClass(Image soundOnImage, Image soundOffImage)
        {
            this.soundOnImage = soundOnImage;
            this.soundOffImage = soundOffImage;

        }
        public void BackgroundSoundOnStart()
        {
            backgroundMusicMediaPlayer.Open(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format(gameMusic, random.Next(1, 4)))));

            backgroundMusicMediaPlayer.Volume = 1;
            backgroundMusicMediaPlayer.Play();
            backgroundMusicMediaPlayer.MediaEnded += new EventHandler(MediaEnded);
        }
        public void MediaEnded(object? sender, EventArgs e)
        {
            backgroundMusicMediaPlayer.Position = TimeSpan.Zero;
            backgroundMusicMediaPlayer.Play();
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
        public void KeyDownMute(object sender, KeyEventArgs e)
        {
            MuteBackgroundMusic();
        }
    }
}
