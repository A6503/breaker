using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game
{
    public enum ObjectRepresentation
    {
        X = -1, // Wall
        O = -2, // Ball
        _ = -3 // Bouncer
    }


    public class GameHandler
    {

        int livesLeft = 3;
        bool gameOver = false;
        bool serveState = true;
        int difficulty = 0;





        private void LifeLost()
        {
            livesLeft -= 1;
            if (livesLeft == 0)
            {
                gameOver = true;

            }
        }



        private void Setup()
        {
            
            Console.WriteLine("Welcome to BlockBreaker");
            Console.WriteLine("(press any key to continue...)");
            Console.ReadKey();
            Console.WriteLine("Break the blocks to win! Move the bouncer with A and D to bounce the ball.");
            Console.WriteLine("(press any key to continue...)");
            Console.ReadKey();
            Console.WriteLine("You have 3 lives.");

            Console.WriteLine("Select map: (1-5)");
            string diff = Console.ReadLine();
            int.TryParse(diff, out difficulty);
            while (difficulty < 1 & difficulty > 5)
            {
                Console.WriteLine("Invalid input. Try again");
                Console.WriteLine("Select map: (1-5)");
                diff = Console.ReadLine();
                int.TryParse(diff, out difficulty);
            }

            Console.WriteLine("Selected map: {1}", difficulty);
            Build(difficulty);
            Console.WriteLine("Press any key to begin");
            Console.ReadKey();
            serveState = true;

        }

        public void StartGame()
        {
            Setup();
            
            while (!gameOver)
            {
                if (serveState)
                {
                    Console.WriteLine("Choose a spot, then hit another key to send the ball.");
                }
                DrawBoard();
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    Move(0);
                }

            }
        }
    }
}