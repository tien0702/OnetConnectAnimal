using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void GenerateAnimals(GameLevelInfo gameLevelInfo)
    {
        int[] cellInfos = new int[Info.GridSize.x * Info.GridSize.y];
        Dictionary<int, int> countTypes = new Dictionary<int, int>();
        for (int i = 0; i < gameLevelInfo.AnimalIDs; i++)
        {
            countTypes.Add(i, 0);
        }
        for (int x = 0; x < Info.GridSize.x; ++x)
        {
            for (int y = 0; y < Info.GridSize.y; ++y)
            {
                int type = -1;
                do
                {
                    type = UnityEngine.Random.Range(0, gameLevelInfo.AnimalIDs);
                } while (countTypes[type] >= gameLevelInfo.CountIDs[type]);

                int index = x * (int)Info.GridSize.y + y;
                cellInfos[index] = type;
                countTypes[type]++;
                this.GetCell(x, y).Init(type);
            }
        }

        Info.CellInfos = cellInfos;
    }

    public void GenerateAnimals(int[] animalIDs)
    {
        for (int i = 0; i < animalIDs.Length; i++)
        {
            _cells[i].Init(animalIDs[i]);
        }
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

    /*public Vector2Int[] FindPath(Vector2Int p1, Vector2Int p2)
    {
        int[,] e = new int[Info.GridSize.x + 2, Info.GridSize.y + 2];
        for(int x = 0; x < Info.GridSize.x; ++x)
        {
            for(int y = 0; y < Info.GridSize.y; ++y)
            {

            }
        }

        //

        // init graph

        Vector2Int size = Info.GridSize + new Vector2Int(2, 2);

        int[,] animalGrid = new int[size.x, size.y];

        for(int x = 0; x < Info.GridSize.x; ++x)
        {
            for(int y = 0 ; y < Info.GridSize.y; ++y)
            {
                int index = x * (int)Info.GridSize.y + y;
                animalGrid[x + 1, y+1] = Info.CellInfos[index];
            }
        }

        Vector2Int[] searchOrder = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };

        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        Vector2Int startPos = p1 + Vector2Int.one;
        Vector2Int endPos = p2 + Vector2Int.one;

        Vector2Int[,] trace = new Vector2Int[size.x, size.y];
        trace[startPos.x, startPos.y] = new Vector2Int(-2, -2);
        for (int i = 0; i < size.x; ++i)
        {
            for(int j = 0 ; j < size.y; ++j)
            {
                trace[i, j] = new Vector2Int(-1, -1);
            }
        }
        
        queue.Enqueue(endPos);

        while(queue.Count > 0)
        {
            var u = queue.First();
            if (u == startPos) break;
            for(int i = 0; i < trace.Length; ++i)
            {
                Vector2Int pos = new Vector2Int(u.x + searchOrder[i].x, u.y + searchOrder[i].y);

                while((pos.x >= 0 && pos.x < size.x) || (pos.y >= 0 && pos.y < size.y) && (animalGrid[pos.x, pos.y] == 0))
                {
                    if (trace[pos.x, pos.y].x == -1)
                    {
                        trace[pos.x, pos.y] = u;
                        queue.Enqueue(pos);
                    }
                }
            }
        }

        // trace back

        List<Vector2Int> res = new List<Vector2Int>();
        if (trace[startPos.x, startPos.y].x != -1)
        {
            while(startPos.x != -2)
            {
                res.Add(startPos - Vector2Int.one);
                startPos = trace[startPos.x, startPos.y];
            }
        }

        return res.ToArray();
    }*/

    public Vector2Int[] FindPath(Vector2Int p1, Vector2Int p2)
    {
        Queue<Tuple<Vector2Int, List<Vector2Int>>> queue = new Queue<Tuple<Vector2Int, List<Vector2Int>>>();
        queue.Enqueue(new Tuple<Vector2Int, List<Vector2Int>>(p1, new List<Vector2Int> { p1 }));

        Vector2Int size = Info.GridSize + new Vector2Int(2, 2);

        int[,] animalGrid = new int[size.x, size.y];

        for (int x = 0; x < size.x; ++x)
            for (int y = 0; y < size.y; ++y)
                animalGrid[x, y] = -1;

        for (int x = 0; x < Info.GridSize.x; ++x)
        {
            for (int y = 0; y < Info.GridSize.y; ++y)
            {
                int index = x * (int)Info.GridSize.y + y;
                animalGrid[x + 1, y + 1] = Info.CellInfos[index];
            }
        }
        /*
                string s = string.Empty;
                for (int x = 0; x < size.x; ++x)
                {
                    for (int y = 0; y < size.y; ++y)
                        s += (animalGrid[x, y]).ToString() + ",";
                    s += "\n";
                }

                Debug.Log(s);*/

        bool[,] visited = new bool[size.x, size.y];
        visited[p1.x, p1.y] = true;

        while (queue.Count > 0)
        {
            Tuple<Vector2Int, List<Vector2Int>> current = queue.Dequeue();
            Vector2Int currentPosition = current.Item1;
            List<Vector2Int> currentPath = current.Item2;

            if (currentPosition.x == p2.x && currentPosition.y == p2.y)
            {
                for(int i = 0; i < currentPath.Count; ++i)
                {
                    currentPath[i] -= new Vector2Int(-1, -1);
                }
                return currentPath.ToArray();
            }

            List<Vector2Int> neighbors = GetNeighbors(currentPosition);
            foreach (Vector2Int neighbor in neighbors)
            {
                if (!IsValid(neighbor, size)) continue;
                if (visited[neighbor.x, neighbor.y]) continue;
                if (animalGrid[neighbor.x, neighbor.y] != -1) continue;

                Debug.Log("ID: " + animalGrid[neighbor.x, neighbor.y].ToString());
                visited[neighbor.x, neighbor.y] = true;
                List<Vector2Int> newPath = new List<Vector2Int>(currentPath) { neighbor };
                queue.Enqueue(new Tuple<Vector2Int, List<Vector2Int>>(neighbor, newPath));
            }
        }

        return null;
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
