using System;
using System.Linq;

namespace Game
{
    public enum BoardObjectRepresentation
    {
        X = -1, // Wall
        O = -2, // Ball
        d = -3, // Bouncer left
        T = -4, // Bouncer middle
        b = -5 // Bouncer right

    }

    public enum Status
    {
        Alive = 0,
        Dead = 1,
        Victory = 2
    }

    public class BoardHandler
    {
        int[,] layout;
        int boardWidth;
        int boardHeight;
        int totalBlocks = 0;
        int gameState = 0;
        int bouncerLocation;
        int[] ballLocation;
        int[] ballDirection;

        static int[] frozen = new int[2] { 0, 0 };
        public BoardHandler(int width, int height)
        {
            layout = new int[width, height];
            boardWidth = width;
            boardHeight = height;
            for (int w = 0; w < width; w++)
            {
                layout[w, 0] = -1;
                if (w == 0 | w == width - 1)
                {
                    for (int h = 1; h < height - 1; h++)
                    {
                        layout[w, h] = -1;
                    }
                }

            }
            bouncerLocation = width / 2;
            ballLocation = new int[2] { width / 2, height - 2 };
            ballDirection = frozen;

        }

        public void BuildBoard(int map)
        {
            UpdateBoard();
            return;
        }

        public void DrawBoard()
        {
            Console.WriteLine();

            Console.WriteLine();
            for (int h = 0; h < boardHeight; h++)
            {
                for (int w = 0; w < boardWidth; w++)
                {
                    if (Enum.IsDefined(typeof(BoardObjectRepresentation), layout[w, h]))
                    {
                        Console.Write("({0})", (BoardObjectRepresentation)layout[w, h]);
                    }
                    else
                    {
                        Console.Write(" {0} ", layout[w, h]);
                    }
                }
                Console.Write("\n");
            }
        }

        private int UpdateBoard()
        {
            // Move the Ball
            BallMovement();
            layout[ballLocation[0], ballLocation[1]] = 0;

            if (ballDirection == frozen) // Ball is on Bouncer
            {
                ballLocation[0] = bouncerLocation;
            }
            else // Ball is moving
            {
                ballLocation[0] += ballDirection[0];
                ballLocation[1] += ballDirection[1];
                
            }

            layout[ballLocation[0], ballLocation[1]] = -2;


            // Renew Bouncer location
            if (bouncerLocation > 1)
            {
                layout[bouncerLocation - 2, boardHeight - 1] = 0;
            }
            layout[bouncerLocation - 1, boardHeight - 1] = -3;
            layout[bouncerLocation, boardHeight - 1] = -4;
            layout[bouncerLocation + 1, boardHeight - 1] = -5;
            if (bouncerLocation < boardHeight - 2)
            {
                layout[bouncerLocation + 2, boardHeight - 1] = 0;
            }



            return 0;
        }

        public int Move(int direction)
        {
            if (bouncerLocation + direction > boardWidth - 2)
            {
                return -1;
            } else if (bouncerLocation + direction < 1)
            {
                return -1;
            }
            bouncerLocation += direction;

            UpdateBoard();
            return 0;
        }

        public int Launch()
        {
            if (ballDirection == frozen)
            {
                ballDirection = new int[] { 0, -1 };
                return 0;
            }
            return -1;
        }

        private int BallMovement()
        {
            if(ballDirection == frozen)
            {
                return 0;
            }

            if (layout[ballLocation[0] + ballDirection[0], ballLocation[1]] < 0) // Check boundary to the side
            {
                ballDirection[0] *= -1;
            }
            else if (layout[ballLocation[0] + ballDirection[0], ballLocation[1]] > 0) // Check breakable to the side
            {
                totalBlocks -= 1;
                layout[ballLocation[0] + ballDirection[0], ballLocation[1]] -= 1;
                ballDirection[0] *= -1;
            }
            else if (layout[ballLocation[0] + ballDirection[0], ballLocation[1] + ballDirection[1]] > 0) // Check breakable to the corner
            {
                layout[ballLocation[0] + ballDirection[0], ballLocation[1] + ballDirection[1]] -= 1;
                ballDirection[0] *= -1;
                ballDirection[1] *= -1;
            }

            if (layout[ballLocation[0], ballLocation[1] - 1] < 0) // Check boundary to the top
            {
                ballDirection[1] *= -1;
            }
            else if (layout[ballLocation[0], ballLocation[1] + ballDirection[1]] > 0) // Check breakable to the top/bottom
            {
                totalBlocks -= 1;
                layout[ballLocation[0] + ballDirection[0], ballLocation[1]] -= 1;
                ballDirection[1] *= -1;
            }

            if (layout[ballLocation[0], ballLocation[1] + 1] < 0) // Check for the bouncer
            {
                if (layout[ballLocation[0], ballLocation[1] + 1] == -3) // Bouncer Left
                {
                    ballDirection = new int[] { -1, -1 };
                } else if (layout[ballLocation[0], ballLocation[1] + 1] == -4) // Bouncer Middle
                {
                    ballDirection = new int[] { 0, -1 };
                } else if (layout[ballLocation[0], ballLocation[1] + 1] == -5) // Bouncer Right
                {
                    ballDirection = new int[] { 1, -1 };
                }
            }
            return 0;
        }

        public int Reset()
        {
            bouncerLocation = boardWidth / 2;
            ballLocation = new int[2] { boardWidth / 2, boardHeight - 1 };
            ballDirection = frozen;
            gameState = 0;
            UpdateBoard();
            return 0;
        }

        public Game.Status GetStatus(){
            return (Status)gameState;
        }

    }
}