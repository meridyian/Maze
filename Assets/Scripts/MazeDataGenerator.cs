using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// will be used only in mazeconstructor
public class MazeDataGenerator
{
    public float placementThreshold;

    public MazeDataGenerator()
    {
        placementThreshold = .1f;
    }

    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        int[,] maze = new int[sizeRows, sizeCols];

        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                // if the current cell is outside of the grid
                // if either index is on the array boundaries
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                {
                    maze[i, j] = 1;
                }
                
                //2 to operate on every other cell
                
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    // to randomly skip this cell and continue iterating through the array
                    if (Random.value > placementThreshold)
                    {
                        // assign 1 for both the current cell and a randomly chosen adjacent cell
                        maze[i, j] = 1;
                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        maze[i + a, j + b] = 1;
                    }
                }
            }
        }
        
        return maze;
    }
}
