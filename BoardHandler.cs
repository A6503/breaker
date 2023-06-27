using System;
using System.Linq;

namespace Game
{
	public class BoardHandler
	{
        int[,] layout;
        int width;
        int height;
        int bouncerLocation;
        int[] ballLocation = { 0, 0 };
        int[] ballDirection = { 0, 0 };
        public static int[,] Build(int width, int height)
		{
			int[,] array = new int[width + 2, height + 2];
			for (int w = 0; w < width; w++)
			{
				array[w, 0] = -1;
				array[w, height + 1] = -1;
				if (w == 0 | w == width + 1)
				{
					for (int h = 1; h < height + 1; h++)
					{
						array[w, h] = -1;
					}
				}

			}
			return array;


		}
	}
}