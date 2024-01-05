using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS
{
    private int[,] _grid;
    private int _sizeX, _sizeY;

    public DFS(int[,] grid)
    {
        this._grid = grid;
        this._sizeX = grid.GetLength(0);
        this._sizeY = grid.GetLength(1);
    }

    public List<List<Vector2Int>> FindAllPaths(Vector2Int p1, Vector2Int p2)
    {
        List<List<Vector2Int>> allPaths = new List<List<Vector2Int>>();
        bool[,] visited = new bool[_sizeX, _sizeY];
        List<Vector2Int> currentPath = new List<Vector2Int>();

        FindDFS(p1, p2, currentPath, allPaths, visited);
        return allPaths;
    }

    void FindDFS(Vector2Int currentNode, Vector2Int destination
        , List<Vector2Int> currentPath, List<List<Vector2Int>> allPaths, bool[,] visited)
    {
        visited[currentNode.x, currentNode.y] = true;
        currentPath.Add(currentNode);

        if (GridUtility.CountTurns(currentPath) <= 2)
        {
            if (currentNode.Equals(destination))
            {
                allPaths.Add(new List<Vector2Int>(currentPath));
            }
            else
            {
                List<Vector2Int> neigbors = GetNeighbors(currentNode);
                foreach (var neighbor in neigbors)
                {
                    if (!IsValid(neighbor, destination) || visited[neighbor.x, neighbor.y]) continue;
                    FindDFS(neighbor, destination, currentPath, allPaths, visited);
                }
            }
        }

        currentPath.RemoveAt(currentPath.Count - 1);
        visited[currentNode.x, currentNode.y] = false;
    }

    List<Vector2Int> GetNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        neighbors.Add(current + Vector2Int.left);
        neighbors.Add(current + Vector2Int.down);
        neighbors.Add(current + Vector2Int.right);
        neighbors.Add(current + Vector2Int.up);

        return neighbors;
    }

    bool IsValid(Vector2Int current, Vector2Int destination)
    {
        if (current.x < 0 || current.x >= _sizeX) return false;
        if (current.y < 0 || current.y >= _sizeY) return false;
        if (_grid[current.x, current.y] != -1 && !current.Equals(destination)) return false;
        return true;
    }
}
