using CubeClimber.classes.logicClasses;
using System.Windows;

namespace CubeClimber
{
    public partial class MainWindow : Window
    {
        GameLogicClass game;
        public MainWindow()
        {
            InitializeComponent();
            game = new GameLogicClass(myCanvas);
            game.OnStartMainWindow();
        }
    }
}
