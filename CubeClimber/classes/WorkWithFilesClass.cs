using CubeClimber.classes.managementClasses;
using System;
using System.IO;

namespace CubeClimber.classes
{
    internal class WorkWithFilesClass
    {
        private readonly string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private readonly string appDirectory;
        private readonly string filePath;
        private readonly ScoreManagementClass scoreManagement = new();
        public WorkWithFilesClass()
        {
            appDirectory = Path.Combine(appDataPath, "CubeClimber");
            filePath = Path.Combine(appDirectory, "high_score.txt");
            OnStartWorkWithFiles();
        }
        public void OnStartWorkWithFiles()
        {
            ReadHighScore();
        }
        private void ReadHighScore()
        {
            if (!Directory.Exists(appDirectory))
            {

                Directory.CreateDirectory(appDirectory);
                WriteTextFile(0);
            }
            if (!File.Exists(filePath))
            {

                WriteTextFile(0);
            }
            else
            {

                TextReader textReader = new StreamReader(filePath);

                scoreManagement.HighScore = Convert.ToInt32(textReader.ReadLine());

                textReader.Close();
            }
        }

        public void UpdateHighScore()
        {

            scoreManagement.UpdateHighScore();
        }

        public void WriteTextFile(int highScore)
        {
            using StreamWriter writer = new(filePath);
            writer.Write(highScore);
            writer.Close();
        }

        public int GetScore()
        {
            return scoreManagement.Score;
        }
        public int GetHighScore()
        {
            return scoreManagement.HighScore;
        }
        public void SetScore()
        {
            scoreManagement.Score = ScoreManagementClass.GetStartScore();
        }
        public int UpdateScore()
        {
            int updatedScore = GetScore() + 1;

            scoreManagement.Score = updatedScore;
            return updatedScore;
        }
    }
}
