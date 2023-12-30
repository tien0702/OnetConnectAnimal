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

        if(!Info.CellInfos.Any(id => id != -1))
        {
            Events.Notify(GridAnimalEvent.OnClear, 0);
        }
    }

    public bool CanConnect(Vector2Int p1,  Vector2Int p2)
    {
        return true;/*
        Vector2Int[] path = FindPath(p1, p2);
        return (path.Length >= 2 && path.Length <= 4);*/
    }

    public Vector2Int[] FindPath(Vector2Int p1, Vector2Int p2)
    {
        int[,] e = new int[Info.GridSize.x + 2, Info.GridSize.y + 2];
        for(int x = 0; x < Info.GridSize.x; ++x)
        {
            for(int y = 0; y < Info.GridSize.y; ++y)
            {

            }
        }

        return new Vector2Int[] { p1, p2 };
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
        for(int i = 0; i < points.Length; ++i)
        {
            result[i] = GetCell(points[i]).transform.position;
        }

        return result;
    }
}
