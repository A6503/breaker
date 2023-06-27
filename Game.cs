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

        private void BuildBoard()
        {
            layout = BoardBuilder.Build(width, height);
            for (int h = 1; h < height+2; h++)
            {
                
                for (int w = 0; w < width+2; w++)
                {
                    if (layout[w, h] == 0)
                    {
                        layout[w, h] = difficulty;
                    }
                }
                difficulty -= 1;
            }
        }


        private void Move(int direction)
        {

        }

        private void LifeLost()
        {
            livesLeft -= 1;
            if (livesLeft == 0)
            {
                gameOver = true;

            }
        }

        private void DrawBoard()
        {
            for (int w = width; w < width+2; w++)
            {
                for (int h = 0; h < height+2; h++)
                {
                    if (Enum.IsDefined(typeof(ObjectRepresentation), layout[w, h]))
                    {
                        Console.Write("[{1}]", (ObjectRepresentation)layout[w, h]);
                    }
                    else
                    {
                        Console.Write("[{1}]", layout[w, h]);
                    }
                }
            }
        }

        private void Start()
        {
            
            Console.WriteLine("Welcome to BlockBreaker");
            Console.WriteLine("(press any key to continue...)");
            Console.ReadKey();
            Console.WriteLine("Break the blocks to win! Move the bouncer with A and D to bounce the ball.");
            Console.WriteLine("(press any key to continue...)");
            Console.ReadKey();
            Console.WriteLine("You have 3 lives.");

            Console.WriteLine("Select difficulty: (1-5)");
            string diff = Console.ReadLine();
            int.TryParse(diff, out difficulty);
            while (difficulty < 1 & difficulty > 5)
            {
                Console.WriteLine("Invalid input. Try again");
                Console.WriteLine("Select difficulty: (1-5)");
                diff = Console.ReadLine();
                int.TryParse(diff, out difficulty);
            }

            Console.WriteLine("Selected Difficulty: {1}", difficulty);
            BuildBoard();
            Console.WriteLine("Press any key to begin");
            Console.ReadKey();
            serveState = true;

        }

        public void Game()
        {
            Start();
            
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