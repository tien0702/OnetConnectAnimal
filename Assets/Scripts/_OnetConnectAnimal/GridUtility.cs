using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUtility
{
    public static int[,] ConvertToTwo(int[] elements, int sizeX, int sizeY)
    {
        int[,] resultArray = new int[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                int index = x * sizeY + y;
                resultArray[x, y] = elements[index];
            }
        }

        return resultArray;
    }

    public static bool FormsTurn(List<Vector2Int> path, Vector2Int newPoint)
    {
        int count = path.Count;

        if (count < 2)
        {
            return false;
        }

        Vector2Int prevPoint = path[count - 2];
        Vector2Int currPoint = path[count - 1];

        return (currPoint.x - prevPoint.x) * (newPoint.y - currPoint.y) != (newPoint.x - currPoint.x) * (currPoint.y - prevPoint.y);
    }

    public static int CountTurns(List<Vector2Int> path)
    {
        int numTurns = 0;

        for (int i = 2; i < path.Count; i++)
        {
            Vector2Int prev = path[i - 2];
            Vector2Int curr = path[i - 1];
            Vector2Int next = path[i];

            if ((curr.x - prev.x) * (next.y - curr.y) != (next.x - curr.x) * (curr.y - prev.y))
            {
                numTurns++;
            }
        }

        return numTurns;
    }
}
