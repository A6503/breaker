using System;
using System.Linq;

namespace Game
{
    public enum BoardObjectRepresentation
    {
        X = -1, // Wall
        O = -2, // Ball
        d = -3 // Bouncer left
        T = -4 // Bouncer middle
        b = -5 // Bouncer right

    }

    public class BoardHandler
	{
        int[,] layout;
        int boardWidth;
        int boardHeight;
        int bouncerLocation;
        int[] ballLocation;
        int[] ballDirection = new int[2] { 0, 0 };
        public BoardHandler(int width, int height)
		{
			layout = new int[width, height];
            boardWidth = width;
            boardHeight = height;
			for (int w = 0; w < width; w++)
			{
				layout[w, 0] = -1;
				layout[w, height + 1] = -1;
				if (w == 0 | w == width + 1)
				{
					for (int h = 1; h < height + 1; h++)
					{
						layout[w, h] = -1;
					}
				}

			}
            bouncerLocation = width / 2;
            ballLocation = new int[2] { width / 2, height - 1 };

        }

        public void BuildBoard(int map)
        {

        }

        public void DrawBoard()
        {
            for (int w = boardWidth; w < boardWidth + 2; w++)
            {
                for (int h = 0; h < boardHeight + 2; h++)
                {
                    if (Enum.IsDefined(typeof(BoardObjectRepresentation), layout[w, h]))
                    {
                        Console.Write("[{1}]", (BoardObjectRepresentation)layout[w, h]);
                    }
                    else
                    {
                        Console.Write("[{1}]", layout[w, h]);
                    }
                }
                Console.Write("\n");
            }
        }

        public int UpdateBoard()
        {
            return 0;
        }

        public int Move(int direction)
        {
            if (bouncerLocation + direction*2)
            return 0;
        }

        public int Launch()
        {
            if (ballDirection == {0,0}) 
            {
                ballDirection = {0,1};
                return 0;
            }
            return -1;
        }
    }
}