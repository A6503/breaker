using System;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Linq;
using System.Xml;

namespace Game
{
    public enum BoardTile
    {
        Empty = 0, // Air
        Wall = -1, // Wall
        Ball = -2, // Ball
        BouncerLeft = -3, // Bouncer left
        BouncerCenter = -4, // Bouncer middle
        BouncerRight = -5 // Bouncer right

    }

    public enum Status
    {
        Alive = 0,
        Dead = 1,
        Victory = 2
    }

    public class BoardHandler
    {


        // Board information
        int[,] layout; // The layout of the board, represented as [x,y]
        int boardWidth;
        int boardHeight;
        int totalBlocks = 0;

        //Current state of the game board
        int difficultyLevel = 1;
        int gameState = 0;
        int score = 0;

        // Bouncer
        int bouncerLocation; // The x position of the center of the Bouncer on the board

        // Ball details
        int[] ballLocation; // The location of the ball on the board
        bool movement = false; // Whether the ball is freely moving or not
        bool yDirection = false; // Is the ball going down? False = going up
        bool xDirection = false; // Is the ball going right? False = going left
        private enum angle
        {
            LowAngle = 0, NormalAngle = 1, HighAngle = 2, // Higher angle means a steeper trajectory
        }
        int ballTime; // The time in updates the ball has been in movement

        public BoardHandler(int width, int height, int difficulty)
        {
            layout = new int[width, height];
            boardWidth = width;
            boardHeight = height;
            difficultyLevel = difficulty;
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
            ballDirection = stillState;

        }

        public void BuildBoard(int map)
        {
            IMapBuilder builder;
            switch (map)
            {
                case 1:
                    builder = new Map1();
                    break;
                case 2:
                    builder = new Map2();
                    break;
                case 3:
                    builder = new Map3();
                    break;
                case 4:
                    builder = new Map4();
                    break;
                case 5:
                    builder = new Map5();
                    break;
                default:
                    return;
            }

            int areawidth = boardWidth - 2;
            int areaheight = boardHeight - 4;
            builder.ObserveSize(areawidth, areaheight);

            for (int h = 1; h < boardHeight - 3; h++)
            {
                for (int w = 1; w < boardWidth - 1; w++)
                {
                    layout[w,h] = builder.PlaceBlock(difficultyLevel);
                }
            }
            totalBlocks = builder.GetBlocks();
            return;
        }

        public void DrawBoard()
        {
            
            for (int lines = 0; lines < 1; lines++)
            {
                Console.WriteLine(lines);
            }
            Console.WriteLine("SCORE: {0}", score);
            for (int h = 0; h < boardHeight; h++)
            {
                for (int w = 0; w < boardWidth; w++)
                {
                    switch ((BoardTile)layout[w, h])
                    {
                        case BoardTile.Empty:
                            Console.Write("   ");
                            break;
                        case BoardTile.Wall:
                            Console.Write("[X]");
                            break;
                        case BoardTile.Ball:
                            Console.Write("(O)");
                            break;
                        case BoardTile.BouncerLeft:
                            Console.Write(" /T");
                            break;
                        case BoardTile.BouncerRight:
                            Console.Write("T\\ ");
                            break;

                        case BoardTile.BouncerCenter:
                            Console.Write("TTT");
                            break;

                        default:
                            Console.Write("[{0}]", layout[w, h]);
                            break;
                     }
                }
                Console.Write("\n");
            }
        }

        private int UpdateBoard()
        {
            // Move the Ball
            CollisionCheck();
            layout[ballLocation[0], ballLocation[1]] = 0;

            if (ballDirection == stillState) // Ball is on Bouncer
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
            if (ballDirection == stillState)
            {
                ballDirection = new int[] { 0, -1 };
                ballTime = 0;
                return 0;
            }
            return -1;
        }

        private int CollisionCheck()
        {
            int xpos = ballLocation[0];
            int ypos = ballLocation[1];
            int xvel = ballDirection[0];
            int xdir;
            if (xvel < 0) { xdir = -1; } else if (xvel > 0) { xdir = 1; } else { xdir = 0; }
            int yvel = ballDirection[1];

            // Check if ball is at the bottom layer
            if (ypos == boardHeight - 2) 
            {
                
                switch (layout[xpos+xdir, ypos + 1])
                {
                    case -3:
                        ballLocation = new int[] { xpos - 1, ypos - 1 };
                        ballDirection = new int[] { -1, -1 };
                        break;
                    case -4:
                        ballLocation = new int[] { xpos+xvel, ypos - 1 };
                        ballDirection = new int[] { xvel, -1 };
                        break;
                    case -5:
                        ballLocation = new int[] { xpos + 1, ypos - 1 };
                        ballDirection = new int[] { 1, -1 };
                        break;
                    case 0:
                        ballLocation = new int[] { xpos + xdir, ypos + 1 };
                        DrawBoard();
                        gameState = 1;
                        return 0;

                    default:
                        gameState = 1;
                        return 0;
                        //break;
                }
            }
            else
            {
                switch (layout[xpos, ypos + yvel])
                {
                    case - 1:
                        yvel *= -1;
                        break;
                    default:
                        yvel *= -1;
                        layout[xpos, ypos + yvel] -= 1;
                        break;
                }
                return 0;
            }
            return -1;

        }

        private void MoveBall()
        {
            ballTime += 1;
            int xMovement = ballDirection[0];
            if (xMovement == 1)
        }

        public int Reset()
        {
            bouncerLocation = boardWidth / 2;
            ballLocation = new int[2] { boardWidth / 2, boardHeight - 1 };
            ballDirection = stillState;
            gameState = 0;
            ballTime = 0;
            UpdateBoard();
            return 0;
        }

        public Game.Status GetStatus(){
            return (Status)gameState;
        }

        public int GetScore()
        {
            return score;
        }

    }
}