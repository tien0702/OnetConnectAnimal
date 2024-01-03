using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstSearch
{
    private int[,] grid;
    private int sizeX, sizeY;
    public DepthFirstSearch(int[,] grid)
    {
        this.grid = grid;
        this.sizeX = grid.GetLength(0);
        this.sizeY = grid.GetLength(1);
    }

    public List<List<Vector2Int>> FindAllPaths(Vector2Int p1, Vector2Int p2)
    {
        List<List<Vector2Int>> allPaths = new List<List<Vector2Int>>();
        List<Vector2Int> currentPath = new List<Vector2Int>();
        bool[,] visited = new bool[sizeX, sizeY];

        FindPathsDFS(p1, p2, currentPath, allPaths, visited);

        return allPaths;
    }

    private void FindPathsDFS(Vector2Int currentPoint, Vector2Int destination, List<Vector2Int> currentPath, List<List<Vector2Int>> allPaths, bool[,] visited)
    {
        // Đánh dấu điểm hiện tại là đã thăm
        visited[currentPoint.x, currentPoint.y] = true;
        currentPath.Add(currentPoint);  

        // Nếu đến được đích, thêm đường đi hiện tại vào danh sách
        if (currentPoint.Equals(destination))
        {
            allPaths.Add(new List<Vector2Int>(currentPath));
        }
        else
        {
            // Kiểm tra các ô kề chưa được thăm
            foreach (Vector2Int neighbor in GetNeighbors(currentPoint))
            {
                if (IsValid(neighbor) && !visited[neighbor.x, neighbor.y] && grid[neighbor.x, neighbor.y] == -1)
                {
                    FindPathsDFS(neighbor, destination, currentPath, allPaths, visited);
                }
            }
        }

        // Quay lui: Bỏ đánh dấu và loại bỏ điểm hiện tại khỏi đường đi
        visited[currentPoint.x, currentPoint.y] = false;
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            new Vector2Int(position.x - 1, position.y),
            new Vector2Int(position.x + 1, position.y),
            new Vector2Int(position.x, position.y - 1),
            new Vector2Int(position.x, position.y + 1)
        };

        return neighbors;
    }

    private bool IsValid(Vector2Int position)
    {
        return position.x >= 0 && position.x < sizeX && position.y >= 0 && position.y < sizeY;
    }
}
