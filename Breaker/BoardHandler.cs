using System;
using System.ComponentModel;
//using System.Formats.Asn1;
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
        BouncerRight = -5, // Bouncer right
        Poof = -6 // Destruction particle

    }

    public enum GameStatus
    {
        Alive = 0,
        Dead = 1,
        Victory = 2,
        Debug = 3
    }

    public class BoardHandler
    {


        // Board information
        private int[,] boardLayout; // The layout of the board, represented as [x,y]
        private int boardWidth; // width of the boardLayout
        private int boardHeight; // height of the boardLayout
        private int totalBlocks = 0; // Total value of all the breakable blocks on the map

        //Current state of the game board
        private int difficultyLevel = 1; // Value that determines toughness of blocks
        GameStatus gameState = GameStatus.Alive; // State of game
        int score = 0; // Accumulated score

        // Bouncer
        int bouncerLocation; // The x position of the center of the Bouncer on the board

        // Ball details
        int[] ballLocation; // The location of the ball on the board
        bool movement = false; // Whether the ball is freely moving or not
        int yDirection = 0; // Movement in y-axis: positive towards the bottom
        int xDirection = 0; // Movement in x-axis: positive towards the right
        private enum Angle
        {
            None = 0, LowAngle = 1, NormalAngle = 2, HighAngle = 3, // Higher angle means a steeper trajectory, None for vertical movement
        }
        Angle ballAngle;
        int ballTime; // The time in updates the ball has been in movement

        /// <summary>
        /// The BoardHandler object is instantiated with the width, height, and difficulty level of the board.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="difficulty"></param>
        public BoardHandler(int width, int height, int difficulty)
        {
            boardLayout = new int[width, height];
            boardWidth = width;
            boardHeight = height;
            difficultyLevel = difficulty;
            for (int w = 0; w < width; w++)
            {
                boardLayout[w, 0] = -1;
                if (w == 0 | w == width - 1)
                {
                    for (int h = 1; h < height - 1; h++)
                    {
                        boardLayout[w, h] = -1;
                    }
                }

            }
            bouncerLocation = boardWidth / 2;
            ballLocation = new int[2] { boardWidth / 2, boardHeight - 1 };
            movement = false; yDirection = 0; xDirection = 0; ballAngle = Angle.None; ballTime = 0;

        }

        /// <summary>
        /// Creates a MapBuilder object to insert breakable blocks into the board,
        /// depending on the map chosen by the player. Stores the total value
        /// of blocks placed in totalBlocks.
        /// </summary>
        /// <param name="map"></param>
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
                case 6:
                    builder = new Map6();
                    break;
                default:
                    return;
            }

            int areawidth = boardWidth - 2;
            int areaheight = boardHeight - 5;
            builder.ObserveSize(areawidth, areaheight);

            for (int h = 1; h < boardHeight - 3; h++)
            {
                for (int w = 1; w < boardWidth - 1; w++)
                {
                    boardLayout[w,h] = builder.PlaceBlock(difficultyLevel);
                }
            }
            totalBlocks = builder.GetBlocks();
            return;
        }

        /// <summary>
        /// Writes a textual representation of the board to the console
        /// </summary>
        public void DrawBoard()
        {
            
            for (int lines = 0; lines < 1; lines++)
            {
                Console.WriteLine("\n");
            }
            Console.WriteLine("     SCORE: {0}", score);
            for (int h = 0; h < boardHeight; h++)
            {
                Console.Write("  ");
                for (int w = 0; w < boardWidth; w++)
                {
                    
                    switch ((BoardTile)boardLayout[w, h])
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
                            Console.Write("///");
                            break;

                        case BoardTile.BouncerCenter:
                            Console.Write("===");
                            break;
                        case BoardTile.BouncerRight:
                            Console.Write("\\\\\\");
                            break;
                        case BoardTile.Poof:
                            Console.Write("RIP");
                            break;
                        default:
                            Console.Write("[{0}]", boardLayout[w, h]);
                            break;
                     }
                }
                Console.Write("\n");
            }
        }
        
        /// <summary>
        /// Updates the values on the board to correspond to the new Ball and Bouncer locations.
        /// Calls BallPhysics to determine the change in position of the Ball.
        /// </summary>
        /// <returns></returns>
        private void UpdateBoard()
        {
            
            // Move the Ball
            
            boardLayout[ballLocation[0], ballLocation[1]] = 0;

            if (movement == false) // Ball is on Bouncer
            {
                ballLocation[0] = bouncerLocation;
                ballLocation[1] = boardHeight - 2;
            }
            else // Ball is moving
            {
                BallPhysics();
            }
            // Set updated Ball position
            boardLayout[ballLocation[0], ballLocation[1]] = -2;


            // Renew Bouncer location
            for (int w = 0; w < boardWidth; w++)
            {
                boardLayout[w, boardHeight - 1] = 0;
            }
            boardLayout[bouncerLocation - 1, boardHeight - 1] = -3;
            boardLayout[bouncerLocation, boardHeight - 1] = -4;
            boardLayout[bouncerLocation + 1, boardHeight - 1] = -5;

            return;
        }

        /// <summary>
        /// Moves the Bouncer depending on the player's input. Then update the board.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>Returns 0 if the move succeeded, or -1 if the bouncer cannot be moved in the selected direciton.</returns>
        public int Move(int direction)
        {
            if (bouncerLocation + direction > boardWidth - 2)
            {
                return -1;
            } 
            else if (bouncerLocation + direction < 1)
            {
                return -1;
            }

            bouncerLocation += direction;
            UpdateBoard();
            return 0;
        }

        /// <summary>
        /// If the ball is on the Bouncer, send it upwards. Otherwise do nothing.
        /// </summary>
        /// <returns></returns>
        public void Launch()
        {
            if (movement == false)
            {
                ballLocation[1] = boardHeight - 3;
                boardLayout[ballLocation[0], boardHeight - 2] = 0;
                yDirection = -1;
                xDirection = 0;
                movement = true;
            }
        }

        /// <summary>
        /// Determines the new angle and direction of the Ball depending on what it collides with.
        /// Blocks it collides with lose 1 durability and adds points (Win if no blocks left).
        /// Assumes that the Ball is in motion.
        /// </summary>
        /// <returns></returns>
        private void BallPhysics()
        {
            int xpos = ballLocation[0];
            int ypos = ballLocation[1];

            bool yFlip = false;
            bool xFlip = false;
            int edgeBounce = 0;
            Angle newAngle = ballAngle;
            Console.WriteLine("{0}", Math.Abs(bouncerLocation - ballLocation[0]));
            // If the ball is at the bottom then die
            if (ypos == boardHeight - 1) 
            {
                Death();
                return;
            }
            else
            {

                if (boardLayout[xpos + xDirection, ypos] != 0 | boardLayout[xpos, ypos + yDirection] != 0) // Check if adjacent tiles are solid
                {
                    // Check right/left side
                    switch (boardLayout[xpos + xDirection, ypos])
                    {
                        case (int)BoardTile.Wall:
                            xFlip = true;
                          
                            break;

                        case > 0:
                            boardLayout[xpos + xDirection, ypos] -= 1;
                            score += 100;
                            totalBlocks -= 1;
                            xFlip = true;
                            break;

                        default:
                            break;
                    }
                    // Check up/down
                    switch (boardLayout[xpos, ypos + yDirection])
                    {
                        case (int)BoardTile.Wall:
                            yFlip = true;
                            break;

                        case (int)BoardTile.BouncerLeft:
                            yFlip = true;
                            edgeBounce = -1;
                            break;
                        case (int)BoardTile.BouncerCenter:
                            yFlip = true;
                            break;
                        case (int)BoardTile.BouncerRight:
                            yFlip = true;
                            edgeBounce = 1;
                            break;

                        case > 0:
                            boardLayout[xpos, ypos + yDirection] -= 1;
                            score += 100;
                            totalBlocks -= 1;
                            yFlip = true;
                            break;

                        default:
                            break;

                    }
                }
                else if (boardLayout[xpos + xDirection, ypos + yDirection] > 0)
                    // Adjacent tiles are clear, so check the corners.
                    // Note that this can't be a Wall, since 
                    // there is always an adjacent tile if the Ball is next to a Wall.
                    
                    switch (boardLayout[xpos + xDirection, ypos + yDirection])
                    {
                        case (int)BoardTile.BouncerLeft:
                            yFlip = true;
                            edgeBounce = -1;

                            break;
                        case (int)BoardTile.BouncerCenter:
                            yFlip = true;
                            break;
                        case (int)BoardTile.BouncerRight:
                            yFlip = true;
                            edgeBounce = 1;

                            break;

                        case > 0:
                            boardLayout[xpos + xDirection, ypos + yDirection] -= 1;
                            score += 100;
                            totalBlocks -= 1;
                            yFlip = true;
                            break;

                        default:
                            gameState = GameStatus.Debug;
                            return;

                    }
            }

            if (xFlip) { xDirection *= -1; }
            if (yFlip) { yDirection *= -1; }

            if (edgeBounce != 0)
            {
                xDirection = edgeBounce;
                if (boardLayout[xpos + xDirection, ypos] != 0)
                {
                    xDirection *= -1;
                }
                int prevAngleInt = (int)(Angle)ballAngle;
                int newAngleInt = (prevAngleInt + 1) % 4;
                newAngle = (Angle)newAngleInt;
            }
            ballAngle = newAngle;
            
            // Finally, move the Ball
            if (totalBlocks == 0) { gameState = GameStatus.Victory; }
            else 
            {
                // Move the ball
                switch (ballAngle)
                {
                    case Angle.None:
                        ballLocation[1] += yDirection;
                        break;

                    case Angle.LowAngle:
                        if (ballTime % 2 == 1)
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
                            ballLocation[0] += xDirection;
                        }
                        ballLocation[1] += yDirection;
                        break;

                    default:
                        break;
                }
                ballTime++;
            }
        }

        /// <summary>
        /// Provides a final check for the location of the Ball
        /// compared to the location of the Bouncer, then changes the game state as appropriate.
        /// If the Bouncer was moved to the correct location at the end,
        /// does not count as a life lost.
        /// </summary>
        private void Death()
        {
            if (Math.Abs(bouncerLocation - ballLocation[0]) < 2)
            {
                ballLocation[1] = boardHeight - 2;
                ballAngle = Angle.None;
                xDirection = 0;
                yDirection = -1;
                return;
            }
            else
            {
                gameState = GameStatus.Dead;
                boardLayout[ballLocation[0], ballLocation[1]] = -6;
                DrawBoard();
            }


        }

        /// <summary>
        /// Resets the status of the board, the locations of the Ball and Bouncer,
        /// and the movement of the Ball.
        /// </summary>
        /// <returns></returns>
        public int Reset()
        {
            gameState = GameStatus.Alive;
            bouncerLocation = boardWidth / 2;
            ballLocation = new int[2] { boardWidth / 2, boardHeight - 1 };
            movement = false; yDirection = 0; xDirection = 0; ballAngle = Angle.None; ballTime = 0;
            UpdateBoard();
            return 0;
        }

        /// <summary>
        /// Returns the current gameState (Alive, Dead, Victory)
        /// </summary>
        /// <returns></returns>
        public GameStatus GetStatus(){
            return gameState;
        }

        // Returns the current Score 
        public int GetScore()
        {
            return score;
        }

        /// <summary>
        /// Inbuilt debugging
        /// </summary>
        public void DebugHelper()
        {
            DrawBoard();
            Console.WriteLine("Bouncer: {0} \n Ball: ({1},{2}), x:{3} y:{4}", bouncerLocation, ballLocation[0], ballLocation[1], xDirection, yDirection);
        }

    }
}