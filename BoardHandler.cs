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
        private int[,] layout; // The layout of the board, represented as [x,y]
        private int boardWidth; // width of the layout
        private int boardHeight; // height of the layout
        private int totalBlocks = 0; // Total value of all the breakable blocks on the map

        //Current state of the game board
        private int difficultyLevel = 1; // Value that determines toughness of blocks
        Status gameState = Status.Alive; // State of game
        int score = 0; // Accumulated score

        // Bouncer
        int bouncerLocation; // The x position of the center of the Bouncer on the board

        // Ball details
        int[] ballLocation; // The location of the ball on the board
        bool movement = false; // Whether the ball is freely moving or not
        int yDirection = 0; // Movement in y-axis
        int xDirection = 0; // Movement in x-axis
        private enum Angle
        {
            None = -1, LowAngle = 0, NormalAngle = 1, HighAngle = 2, // Higher angle means a steeper trajectory, None means vertical movement
        }
        Angle angle;
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
            movement = false; yDirection = 0; xDirection = 0; angle = Angle.None; ballTime = 0;

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

            if (movement == false) // Ball is on Bouncer
            {
                ballLocation[0] = bouncerLocation;

            }
            else // Ball is moving
            {
                switch (angle){
                    case Angle.None:
                        ballLocation[1] += yDirection;
                        break;

                    case Angle.LowAngle:
                        if (ballTime%2 == 1)
                        {
                            ballLocation[1] += yDirection;
                        }
                        ballLocation[0] += xDirection;
                        break;

                    case Angle.NormalAngle:
                        ballLocation[0] += xDirection;
                        ballLocation[1] += yDirection;
                        break;

                    case Angle.HighAngle:
                        if (ballTime % 2 == 1)
                        {
                            ballLocation[0] +=xDirection;
                        }
                        ballLocation[1] += yDirection;
                        break;

                    default:
                        break;
                }
                ballTime++;
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
            if (movement == false)
            {
                yDirection = -1;
                movement = true;
                return 0;
            }
            return -1;
        }

        private int CollisionCheck()
        {
            int xpos = ballLocation[0];
            int ypos = ballLocation[1];

            // Check if ball is at the bottom layer
            if (ypos == boardHeight - 2) 
            {
                
                switch (layout[xpos, ypos + 1])
                {
                    case -3:
                        yDirection = -1;
                        break;
                    case -4:
                        ballDirection = new int[] { xvel, -1 };
                        break;
                    case -5:
                        ballDirection = new int[] { 1, -1 };
                        break;
                    case 0:
                        DrawBoard();
                        gameState = Status.Dead;
                        return 0;

                    default:
                        gameState = Status.Dead;
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
            if (totalBlocks == 0)
            {
                gameState = Status.Victory;
                return 0;
            }
            return -1;

        }


        public int Reset()
        {
            bouncerLocation = boardWidth / 2;
            ballLocation = new int[2] { boardWidth / 2, boardHeight - 1 };
            movement = false; yDirection = 0; xDirection = 0; angle = Angle.None; ballTime = 0;
            UpdateBoard();
            return 0;
        }

        public Status GetStatus(){
            return gameState;
        }

        public int GetScore()
        {
            return score;
        }

    }
}