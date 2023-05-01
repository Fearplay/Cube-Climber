using System;
using System.Windows.Media;

namespace CubeClimber.classes.soundScripts
{
    internal class NextRoundSoundClass
    {
        private const string NextRoundSound = @"sounds/next_round.wav";
        private readonly MediaPlayer nextRoundMediaPlayer = new();
        public void PlayNextRoundSound()
        {
            nextRoundMediaPlayer.Volume = 0.3;
            nextRoundMediaPlayer.Play();
        }
        public void StopNextRoundSound()
        {
            nextRoundMediaPlayer.Volume = 0;
            nextRoundMediaPlayer.Open(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NextRoundSound)));
            nextRoundMediaPlayer.Stop();
        }
    }
}
