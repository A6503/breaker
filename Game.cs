using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game
{



    public class GameHandler
    {

        int livesLeft;
        bool gameOver;
        bool serveState;
        int mapLevel;
        BoardHandler? gameBoard;

        public GameHandler()
        {
            livesLeft = 3;
            gameOver = false;
            serveState = true;
            mapLevel = 0;
        }

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

            // Player chooses a preset map
            Console.WriteLine("Select map: (1-5)");
            string mapSelect = Console.ReadLine();
            int.TryParse(mapSelect, out mapLevel);
            while (mapLevel < 1 & mapLevel > 5)
            {
                Console.WriteLine("Invalid input. Try again");
                Console.WriteLine("Select map: (1-5)");
                mapSelect = Console.ReadLine();
                int.TryParse(mapSelect, out mapLevel);
            }

            Console.WriteLine("Selected map: {1}", mapLevel);
            
            gameBoard.Build(mapLevel);
            Console.WriteLine("Press any key to begin");
            Console.ReadKey();
            serveState = true;

        }

        public void StartGame()
        {
            // Prepare the Board
            gameBoard = new BoardHandler(10, 10); // Size
            Setup();
            
            while (!gameOver)
            {
                if (serveState)
                {
                    Console.WriteLine("Choose a spot, then press W to send the ball.");
                }
                gameBoard.DrawBoard();
                Thread.Sleep(400);
                if (Console.KeyAvailable){
                    ConsoleKeyInfo playerMove = Console.ReadKey();
                    if (playerMove.Key = ConsoleKey.W){
                        gameBoard.Launch();
                    }
                    else if (playerMove.Key == ConsoleKey.A){
                        gameBoard.Move(-1);
                    }else if (playerMove.Key = ConsoleKey.D){
                        gameBoard.Move(1);
                    }else{
                        gameBoard.Move(0);
                    }
                    
                }else{
                    gameBoard.Move(0);
                }


            }
        }
    }
}