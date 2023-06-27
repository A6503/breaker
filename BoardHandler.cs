using System;
using System.Linq;

namespace Game
{
    public enum ObjectRepresentation
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
        int width;
        int height;
        int bouncerLocation;
        int[] ballLocation = { 0, 0 };
        int[] ballDirection = { 0, 0 };
        public void Build(int width, int height)
		{
			layout = new int[width + 2, height + 2];
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


		}

        public void DrawBoard()
        {
            for (int w = width; w < width + 2; w++)
            {
                for (int h = 0; h < height + 2; h++)
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
                Console.Write("\n");
            }
        }

        public int Move(int direction)
        {
            return 0;
        }
    }
}