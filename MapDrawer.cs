using Game;
using System;

interface IMapBuilder
{
	/// <summary>
	/// Determines what the block at the current location should be.
	/// Returns the value of the block.
	/// </summary>
	int PlaceBlock(int difficulty);

	/// <summary>
	/// Provides the MapBuilder with the width and height of
	/// the area in which blocks can be placed. NOT the actual 
	/// size of the Board.
	/// </summary>
	/// <param name="width"">Width of the placeable area.</param>
	/// <param name="height">Height of the placeable area.</param>
	void ObserveSize(int width, int height);

    /// <summary>
    /// Returns the sum of the values of the placed blocks (it takes x bounces to destroy a block with a value of x)
    /// </summary>
	int GetBlocks();
}
public class Map1 : IMapBuilder
{
    int maxWidth = 0;
    int maxHeight = 0;
    int blocksPlaced = 0;
    public int PlaceBlock(int difficulty)
    {
		blocksPlaced += difficulty;
        return difficulty;
    }
    public void ObserveSize(int width, int height)
    {
        maxWidth = width; maxHeight = height;
        return;
    }
    public int GetBlocks()
    {
        return blocksPlaced;
    }
}
public class Map2: IMapBuilder
{
	int maxWidth = 0;
	int maxHeight = 0;
	int iterationCounter = 0;
	int blocksPlaced = 0;
	public int PlaceBlock(int difficulty)
	{
		//Console.WriteLine((iterationCounter / maxWidth) % 2);
		if ((iterationCounter / maxWidth) % 2 == 1)
		{
			iterationCounter++;
			return 0;
		}
		blocksPlaced += difficulty+3;
		iterationCounter++;
		return difficulty+3;
	}
	public void ObserveSize(int width, int height)
	{
		maxWidth = width; maxHeight = height;
		return;
	}
	public int GetBlocks()
	{
		return blocksPlaced;
	}
}

public class Map3 : IMapBuilder
{
    int maxWidth = 0;
    int maxHeight = 0;
    int iterationCounter = 0;
    int blocksPlaced = 0;
    public int PlaceBlock(int difficulty)
    {
        //Console.WriteLine((iterationCounter / maxWidth) % 2);
        if ((iterationCounter / maxWidth) % 3 != 1)
        {
            if((iterationCounter % maxWidth) % 3 == 1)
            {
                iterationCounter++;
                blocksPlaced += difficulty + 2;
                return difficulty + 2;
            }
            else
            {
                iterationCounter++;
                return 0;
            }

        }
        else
        {
            blocksPlaced += difficulty + 2;
            iterationCounter++;
            return difficulty + 2;
        }
        
    }
    public void ObserveSize(int width, int height)
    {
        maxWidth = width; maxHeight = height;
        return;
    }
    public int GetBlocks()
    {
        return blocksPlaced;
    }
}
public class Map4 : IMapBuilder
{
    int maxWidth = 0;
    int maxHeight = 0;
    int iterationCounter = 0;
    int blocksPlaced = 0;
    public int PlaceBlock(int difficulty)
    {
        //Console.WriteLine((iterationCounter / maxWidth) % 2);
        if (iterationCounter % 5 == 0)
        {
            blocksPlaced += difficulty*3;
            iterationCounter++;
            return difficulty*3;
        }
        iterationCounter++;
        return 0;
    }
    public void ObserveSize(int width, int height)
    {
        maxWidth = width; maxHeight = height;
        return;
    }
    public int GetBlocks()
    {
        return blocksPlaced;
    }
}

public class Map5 : IMapBuilder
{
    int maxWidth = 0;
    int maxHeight = 0;
    int iterationCounter = 0;
    int blocksPlaced = 0;
    public int PlaceBlock(int difficulty)
    {
        //Console.WriteLine((iterationCounter / maxWidth) % 2);
        if ((iterationCounter / maxWidth) < maxHeight/3 | (iterationCounter / maxWidth)+1 > maxHeight - (maxHeight / 3))
        {
            if ((iterationCounter % maxWidth) < (maxWidth / 3) + 1 | (iterationCounter % maxWidth)+1 > maxWidth - (maxWidth / 3) - 1)
            {
                iterationCounter++;
                blocksPlaced += difficulty * 2;
                return difficulty * 2;
            }
            else
            {
                iterationCounter++;
                return 0;
            }

        }
        else
        {
            if ((iterationCounter % maxWidth) < maxWidth / 5 | (iterationCounter % maxWidth)+1 > maxWidth - (maxWidth / 5))
            {
                iterationCounter++;
                blocksPlaced += difficulty * 2;
                return difficulty * 2;
            }
            else
            {
                iterationCounter++;
                return 0;
            }
        }

    }
    public void ObserveSize(int width, int height)
    {
        maxWidth = width; maxHeight = height;
        return;
    }
    public int GetBlocks()
    {
        return blocksPlaced;
    }
}
