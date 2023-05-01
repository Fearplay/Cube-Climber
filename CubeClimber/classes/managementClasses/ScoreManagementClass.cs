namespace CubeClimber.classes.managementClasses
{
    internal class ScoreManagementClass
    {
        private const int StartHighScore = 0;
        private const int StartScore = 0;
        private int score = StartScore;
        private int highScore = StartHighScore;

        public int HighScore { get => highScore; set => highScore = value; }
        public int Score { get => score; set => score = value; }
        public ScoreManagementClass()
        {

        }
        public void UpdateHighScore()
        {

            if (Score > HighScore)
            {
                HighScore = Score;
                WorkWithFilesClass workWithFiles = new();
                workWithFiles.WriteTextFile(HighScore);
            }
        }
        public static int GetStartScore()
        {
            return StartScore;
        }


    }
}
