using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Game
{


    public class GameHandler
    {
        static int size = 25; // Width and height to build the board

        int livesLeft; // How many lives remaining
        bool gameOver; // If true, end the game
        bool serveState; // If true, the ball does not move, and the console waits for player input
        int mapStyle; // Which map to draw
        int difficulty; // The higher the value, the harder it is to break the blocks

        BoardHandler gameBoard = new BoardHandler(0, 0, 0); // The gameBoard object representing the board

        public GameHandler(int level)
        {
            livesLeft = 3;
            gameOver = false;
            serveState = true;
            mapStyle = 0;
            difficulty = level;
        }
        private void Setup()
        {
            
            Console.WriteLine("Welcome to BlockBreaker");
            Console.WriteLine("(press any key to continue...)");
            Console.ReadKey();
            Console.WriteLine("\nBreak the blocks to win! Move the bouncer with A and D to bounce the ball.");
            Console.WriteLine("Type X to exit");
            Console.WriteLine("(press any key to continue...)");
            Console.ReadKey();
            Console.WriteLine("\nYou have 3 lives.");

            // Player chooses a preset map
            Console.WriteLine("Select map: (1-5)");
            var mapSelect = Console.ReadLine();
            int.TryParse(mapSelect, out mapStyle);
            while (mapStyle < 1 | mapStyle > 5)
            {
                Console.WriteLine("Invalid input. Try again");
                Console.WriteLine("Select map: (1-5)");
                mapSelect = Console.ReadLine();
                int.TryParse(mapSelect, out mapStyle);
            }

            Console.WriteLine("Selected map: {0}", mapStyle);

            // Initialize the GameBoard
            gameBoard = new BoardHandler(size, size, difficulty);
            gameBoard.BuildBoard(mapStyle);

            Console.WriteLine("Press any key to begin");
            Console.ReadKey();
            serveState = true;

        }

        public void StartGame()
        {
            // Prepare the Board

            Setup();
            
            while (!gameOver)
            {
                if (serveState)
                {
                    Console.WriteLine("Choose a spot, then press W to send the ball.");
                    while (serveState)
                    {
                        gameBoard.Move(0);
                        gameBoard.DrawBoard();
                        ConsoleKeyInfo playerMove = Console.ReadKey();
                        switch (playerMove.Key)
                        {
                            case ConsoleKey.W:
                                gameBoard.Launch();
                                serveState = false;
                                break;
                            case ConsoleKey.X:
                                gameOver = true;
                                break;
                            case ConsoleKey.A:
                                gameBoard.Move(-1);
                                break;
                            case ConsoleKey.D:
                                gameBoard.Move(1);
                                break;
                            default:
                                gameBoard.Move(0);
                                break;
                        }
                    }

                }

                gameBoard.DrawBoard();
                Thread.Sleep(200);
                if (Console.KeyAvailable)
                {
                        ConsoleKeyInfo playerMove = Console.ReadKey();
                        switch (playerMove.Key)
                        {
                            case ConsoleKey.A:
                                gameBoard.Move(-1);
                                break;
                            case ConsoleKey.D:
                                gameBoard.Move(1);
                                break;
                            default:
                                gameBoard.Move(0);
                                break;
                        }
                }
                else
                {
                    gameBoard.Move(0);
                }

                switch (gameBoard.GetStatus())
                {
                    case Status.Dead:
                        LifeLost();
                        break;
                    case Status.Victory:
                        Victory();
                        break;
                    case Status.Alive:
                        break;
                    default:
                        break;
                }
                Console.Clear();


            }
            Console.WriteLine("The Game is over");
            Console.ReadKey();
        }

        /// <summary>
        /// Subtract 1 from the number of lives, and write a
        /// message to the console about how many lives remain.
        /// Then, return to serve state.
        /// If no lives remain, the game ends.
        /// </summary>
        private void LifeLost()
        {
            livesLeft -= 1;
            if (livesLeft == 0)
            {
                gameOver = true;
                Console.WriteLine("You ran out of Lives!");
            }
            else
            {
                Console.WriteLine("You lost a life!");
                Console.WriteLine("{0} lives remain.", livesLeft);
                gameBoard.Reset();
                serveState = true;
            }
            return;
        }

        private void Victory()
        {
            Console.WriteLine("You win!");
        }
    }
}