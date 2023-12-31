using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public class GridInfo
{
    public Vector2Int GridSize;
    public Vector2 CellOffset;
    public Vector2 CellSize;
    public bool UseCellPrefabSize;
    public string CellPrefabPath;

    public int[] CellInfos;
}

public class GridController : MonoBehaviour
{
    [field: SerializeField] public GridInfo Info { private set; get; }

    [SerializeField] protected List<CellController> _cells;
    protected CellController _cellPrefab;

    public virtual bool Init(GridInfo info)
    {
        if (info == null) return false;

        this.Info = info;
        _cellPrefab = Resources.Load<CellController>(info.CellPrefabPath);

        if (_cells == null || _cells.Count == 0)
        {
            GenerateGrid();
            GenerateCellItems();
        }
        return true;
    }

    public CellController GetCell(int x, int y)
    {
        if (x < 0 || x >= Info.GridSize.x || y < 0 || y >= Info.GridSize.y)
        {
            Debug.Log("Out of index");
            return null;
        }
        int index = x * (int)Info.GridSize.y + y;

        return _cells[index];
    }

    public CellController GetCell(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= Info.GridSize.x || pos.y < 0 || pos.y >= Info.GridSize.y)
        {
            Debug.Log("Out of index");
            return null;
        }
        int index = pos.x * (int)Info.GridSize.y + pos.y;

        return _cells[index];
    }

    public virtual void RemoveAt(Vector2Int pos)
    {
        var cell = this.GetCell(pos);
        if (cell == null)
        {
            return;
        }
        int index = pos.x * (int)Info.GridSize.y + pos.y;
        Destroy(cell.gameObject);
        _cells[index] = null;
    }

    public virtual void AddAt(int x, int y, CellController cell)
    {
        if (x < 0 || x >= Info.GridSize.x || y < 0 || y >= Info.GridSize.y)
        {
            Debug.Log("Out of index");
            return;
        }
        int index = x * (int)Info.GridSize.y + y;

        _cells[index] = cell;
    }

    public virtual void GenerateGrid()
    {
        if (_cells != null && _cells.Count > 0)
        {
            foreach (var cell in _cells)
            {
                Destroy(cell.gameObject);
            }
            _cells.Clear();
        }

        if (Info.UseCellPrefabSize)
        {
            SpriteRenderer cellSpriteRenderer = _cellPrefab.GetComponent<SpriteRenderer>();
            Info.CellSize = cellSpriteRenderer.bounds.size;
        }

        Vector3 firstPosition = Vector3.zero;
        firstPosition.x = (Info.CellSize.x + Info.CellOffset.x) * (Info.GridSize.x - 1);
        firstPosition.y = (Info.CellSize.y + Info.CellOffset.y) * (Info.GridSize.y - 1);

        firstPosition *= -(0.5f);

        for (int x = 0; x < Info.GridSize.x; x++)
        {
            for (int y = 0; y < Info.GridSize.y; y++)
            {
                Vector3 cellPosition = firstPosition + new Vector3(x * (Info.CellSize.x + Info.CellOffset.x), y * (Info.CellSize.y + Info.CellOffset.y), 0);
                CellController cell = Instantiate(_cellPrefab, transform);
                cell.transform.localPosition = cellPosition;
                cell.CellPos = new Vector2Int(x, y);
                cell.gameObject.name = string.Format("[{0}, {1}]", x, y);
                _cells.Add(cell);
            }
        }
    }

    public virtual void GenerateCellItems()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            _cells[i].Init(Info.CellInfos[i]);
        }
    }
}
