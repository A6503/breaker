using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;


namespace ConsoleApp1
{
    internal class Program
    {

        static void Main(string[] args)
        {

            bool quit = false;
            while (!quit)
            {
                Game.Game play = new Game.Game();
                int score = play.PlayGame();

                


                try
                {
                    using (var highScoreWriter = new StreamWriter("HighScores.txt", true))
                    {
                        highScoreWriter.Write("");
                    }
                    
                    HighScoreFormat[] highScores = new HighScoreFormat[10];
                    Array.Fill(highScores, new HighScoreFormat(""));
                    bool updateScores = false;
                    int count = 0;
                    using (var scoreComparer = new StreamReader("HighScores.txt"))
                    {
                        scoreComparer.ReadLine();

                        while (count < 10)
                        {
                            HighScoreFormat highScore;
                            highScore = new HighScoreFormat(scoreComparer.ReadLine());
                            //Console.WriteLine(highScore.ToString());
                            if (highScore.IsEmpty())
                            {
                                if (updateScores) { break; }
                                Console.WriteLine("New High Score reached! \n Enter your name:");
                                string name = "[" + Console.ReadLine() + "]";
                                highScores[count] = new HighScoreFormat(name, score);
                                updateScores = true;
                                break;
                            }

                            else
                            {
                                if (highScore.GetScore() < score & !updateScores)
                                {
                                    Console.WriteLine("New High Score reached! \n Enter your name:");
                                    string name = "[" + Console.ReadLine() + "]";
                                    highScores[count] = new HighScoreFormat(name, score);
                                    count++;
                                    highScores[count] = highScore;
                                    updateScores = true;
                                    count++;
                                }
                                else
                                {
                                    highScores[count] = highScore;
                                    count++;
                                }
                            }
                        }
                        
                    }
                    if (updateScores) // Clear the file then rewrite
                    {
                        using (StreamWriter outputFile = new StreamWriter("HighScores.txt", false))
                        {

                            outputFile.WriteLine("         DATE         |  NAME  |  SCORE  ");

                        }
                        using (StreamWriter outputFile = new StreamWriter("HighScores.txt", true))
                        {
                            
                            for (int i = 0; i < highScores.Length; i++)
                            {
                                if (!highScores[i].IsEmpty())
                                {
                                    outputFile.WriteLine(highScores[i].ToString());
                                }
                                
                            }
                                
                        }
                    }

                    Console.WriteLine("HIGHSCORES: ");
                    using (var scoreReader = new StreamReader("HighScores.txt"))
                    {
                        Console.WriteLine(scoreReader.ReadToEnd());
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("Highscores file could not be read:");
                    Console.WriteLine(e.Message);
                }


                Console.WriteLine("Play again? (type Y)");
                ConsoleKeyInfo playAgain = Console.ReadKey();
                switch(playAgain.Key)
                {
                    case ConsoleKey.Y:
                        Console.WriteLine();
                        break;
                    default:
                        quit = true;
                        break;
                }
            }

            return;

        }
    }
    internal class HighScoreFormat
    {
        string name = "[]";
        string time = " - ";
        int score = 0;
        bool isEmpty = true;


        /// <summary>
        /// Takes a string and converts it to a HighScoreFormat
        /// Creates an empty format if the string is not in the proper format,
        /// or if the score value is zero.
        /// </summary>
        /// <param name="scoreFormat"></param>
        public HighScoreFormat(string? scoreFormat)
        {
            if (scoreFormat == null) { return; }

            string[] time_name_score = scoreFormat.Split(" - ");
            if (time_name_score.Length != 3) { return; }

            int.TryParse(time_name_score[2], out score);
            if (score == 0) { return; }

            time = time_name_score[0];
            name = time_name_score[1];

            isEmpty = false;
        }

        /// <summary>
        /// Creates a new HighScoreFormat object with the current time,
        /// given a string newName and an integer newScore.
        /// Creates an empty format if the score value is zero.
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="newScore"></param>
        public HighScoreFormat(string newName, int newScore) 
        {
            
            score = newScore;
            if (score == 0)
            {
                return;
            }
            name = newName;
            DateTime now = DateTime.Now;
            time = now.ToString();
            isEmpty = false;
        }

        /// <summary>
        /// Returns the string representation of the HighScoreFormat
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (isEmpty)
            {
                return time + " - " + name + " - ";
            }
            return time + " - " + name + " - " + score;
        }

        public int GetScore() {  return score; }

        public bool IsEmpty() { return  isEmpty; }
    }
}
