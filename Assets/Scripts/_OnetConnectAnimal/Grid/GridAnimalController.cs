using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    DFS _dfs;

    public int[] RandomAnimals(GameLevelInfo gameLevelInfo)
    {
        int[] animalIDs = new int[base.Info.GridSize.x * base.Info.GridSize.y];
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
                animalIDs[index] = type;
                countTypes[type]++;
            }
        }

        return animalIDs;
    }

    public Tuple<Vector2Int, int[]> GridExtend(int[] cellInfos, Vector2Int originSize)
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
                    result[i * newSize.y + j] = cellInfos[(i - 1) * originSize.y + (j - 1)];
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
        _dfs = new DFS(GridUtility.ConvertToTwo(Info.CellInfos, Info.GridSize.x, Info.GridSize.y));
        var paths = _dfs.FindAllPaths(p1, p2);

        if (paths == null || paths.Count == 0) return null;

        int index = 0;
        for (int i = 1; i < paths.Count; ++i)
        {
            if (paths[i].Count < paths[index].Count)
            {
                index = i;
            }
        }

        return paths[index].ToArray();
    }

    public Tuple<Vector2Int, Vector2Int> GetHint()
    {

        return null;
    }

    public void DrawLine(Vector3[] points, Action callbackOnCompleted)
    {
        LineRenderer line = Instantiate<LineRenderer>(_linePrefab);

        line.positionCount = points.Length;
        line.SetPositions(points);
        AudioManager.Instance.PlaySFX("linked");
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
