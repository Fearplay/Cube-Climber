using System;
using System.Windows.Media;

namespace CubeClimber.classes.soundScripts
{

    internal class HitSoundClass
    {
        private const string HitSound = @"sounds/hit.wav";
        private readonly MediaPlayer hitMediaPlayer = new();
        public void PlayHitSound()
        {
            hitMediaPlayer.Open(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HitSound)));
            hitMediaPlayer.Volume = 0.3;
            hitMediaPlayer.Play();
        }
    }
}
