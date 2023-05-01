using CubeClimber.classes.soundScripts;
using System.Windows.Controls;

namespace CubeClimber.classes.logicClasses
{
    public class SoundLogicClass
    {
        private readonly HitSoundClass hitSound = new();
        private readonly NextRoundSoundClass nextRoundSound = new();
        private readonly BackgroundSoundClass? backgroundSound;

        public SoundLogicClass(Image soundOnImage, Image soundOffImage, MainWindow mainWindow)
        {
            backgroundSound = new BackgroundSoundClass(soundOnImage, soundOffImage);
            mainWindow.KeyDown += backgroundSound.KeyDownMute;
            BackgroundSoundOnStart();
        }
        private void BackgroundSoundOnStart()
        {
            backgroundSound?.BackgroundSoundOnStart();
        }
        public void PlayNextRoundSound()
        {
            nextRoundSound.PlayNextRoundSound();
        }
        public void StopNextRoundSound()
        {
            nextRoundSound.StopNextRoundSound();
        }
        public void PlayHitSound()
        {
            hitSound.PlayHitSound();
        }
    }
}
