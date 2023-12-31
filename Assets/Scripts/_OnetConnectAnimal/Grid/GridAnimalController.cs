using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TT;
using UnityEngine;

public class GridAnimalController : GridController
{
    #region Events

    public enum GridAnimalEvent { OnAdd, OnRemove, OnClear };

    public ObserverEvents<GridAnimalEvent, int> Events { get; private set; }
        = new ObserverEvents<GridAnimalEvent, int>();

    #endregion 

    [SerializeField] LineRenderer _linePrefab;
    [SerializeField] GameObject _removeEf;
    [SerializeField] string _removeSoundName;

    public GameObject p;

    public void GenerateAnimals(GameLevelInfo gameLevelInfo)
    {
        int[] _cellInfos = new int[base.Info.GridSize.x * base.Info.GridSize.y];
        Dictionary<int, int> countTypes = new Dictionary<int, int>();
        for (int i = 0; i < gameLevelInfo.AnimalIDs; i++)
        {
            countTypes.Add(i, 0);
        }
        for (int x = 0; x < base.Info.GridSize.x; ++x)
        {
            for (int y = 0; y < base.Info.GridSize.y; ++y)
            {
                int type = -1;
                do
                {
                    type = UnityEngine.Random.Range(0, gameLevelInfo.AnimalIDs);
                } while (countTypes[type] >= gameLevelInfo.CountIDs[type]);

                int index = x * (int)base.Info.GridSize.y + y;
                _cellInfos[index] = type;
                countTypes[type]++;
            }
        }
    }

    public Tuple<Vector2Int, int[]> GridExtend(int[] cellInfoss, Vector2Int originSize)
    {
        Vector2Int newSize = originSize + new Vector2Int(2, 2);

        int[] result = new int[newSize.x * newSize.y];

        for (int i = 0; i < newSize.x; i++)
        {
            for (int j = 0; j < newSize.y; j++)
            {
                if (i == 0 || i == newSize.x - 1 || j == 0 || j == newSize.y - 1)
                {
                    result[i * newSize.y + j] = -1;
                }
                else
                {
                    result[i * newSize.y + j] = cellInfoss[(i - 1) * originSize.y + (j - 1)];
                }
            }
        }

        return new Tuple<Vector2Int, int[]>(newSize, result);
    }

    public override void RemoveAt(Vector2Int pos)
    {
        var cell = this.GetCell(pos);
        if (cell == null)
        {
            return;
        }
        int index = pos.x * (int)Info.GridSize.y + pos.y;
        Events.Notify(GridAnimalEvent.OnRemove, _cells[index].ID);

        Info.CellInfos[index] = -1;
        _cells[index].Init(-1);

        if (!Info.CellInfos.Any(id => id != -1))
        {
            Events.Notify(GridAnimalEvent.OnClear, 0);
        }
    }

    public Vector2Int[] FindPath(Vector2Int p1, Vector2Int p2)
    {
        Queue<Tuple<Vector2Int, List<Vector2Int>>> queue = new Queue<Tuple<Vector2Int, List<Vector2Int>>>();
        queue.Enqueue(new Tuple<Vector2Int, List<Vector2Int>>(p1, new List<Vector2Int> { p1 }));

        Vector2Int size = Info.GridSize;

        bool[,] visited = new bool[size.x, size.y];
        visited[p1.x, p1.y] = true;

        while (queue.Count > 0)
        {
            Tuple<Vector2Int, List<Vector2Int>> current = queue.Dequeue();
            Vector2Int currentPosition = current.Item1;
            List<Vector2Int> currentPath = current.Item2;

            if (currentPosition.Equals(p2))
            {
                return currentPath.ToArray();
            }

            List<Vector2Int> neighbors = GetNeighbors(currentPosition);
            foreach (Vector2Int neighbor in neighbors)
            {
                if (!IsValid(neighbor, size)) continue;
                if (visited[neighbor.x, neighbor.y]) continue;
                if (GetCell(neighbor).ID != -1 && !neighbor.Equals(p2)) continue;
                var node = Instantiate(p, null);
                node.transform.position = GetCell(neighbor).transform.position;

                visited[neighbor.x, neighbor.y] = true;
                List<Vector2Int> newPath = new List<Vector2Int>(currentPath) { neighbor };
                queue.Enqueue(new Tuple<Vector2Int, List<Vector2Int>>(neighbor, newPath));
            }
        }

        return null;
    }

    public int CountTurns(Vector2Int[] path)
    {
        int numTurns = 0;

        for (int i = 2; i < path.Length; i++)
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

    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        neighbors.Add(new Vector2Int(position.x - 1, position.y));
        neighbors.Add(new Vector2Int(position.x + 1, position.y));
        neighbors.Add(new Vector2Int(position.x, position.y - 1));
        neighbors.Add(new Vector2Int(position.x, position.y + 1));

        return neighbors;
    }

    private bool IsValid(Vector2Int position, Vector2Int size)
    {
        return position.x >= 0 && position.x < size.x && position.y >= 0 && position.y < size.y;
    }

    public Vector2Int Hint()
    {
        return default(Vector2Int);
    }

    public void DrawLine(Vector3[] points, Action callbackOnCompleted)
    {
        LineRenderer line = Instantiate<LineRenderer>(_linePrefab);

        line.positionCount = points.Length;
        line.SetPositions(points);
        LeanTween.delayedCall(0.5f, callbackOnCompleted);
        LeanTween.delayedCall(0.5f, () => { Destroy(line.gameObject); });
    }

    public Vector3[] ConvertPathToPoints(Vector2Int[] points)
    {
        Vector3[] result = new Vector3[points.Length];
        for (int i = 0; i < points.Length; ++i)
        {
            result[i] = GetCell(points[i]).transform.position;
        }

        return result;
    }
}
