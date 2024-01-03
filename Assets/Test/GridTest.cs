using System.Collections;
using System.Collections.Generic;
using TMPro;
using TT;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    public enum SelectionMode { ChangeValue, SelectNode }

    public SelectionMode selectionMode;

    public CellTest prefab;
    public GameObject flag;
    public int sizeX, sizeY;
    public Vector2 CellOffset;
    public int ranMin, ranMax;
    public float drawSpeed;

    public TextMeshProUGUI _modeTxt, _numPathTxt;

    DFS _dfs;
    int[,] grid;

    CellTest cell1, cell2;
    GameObject flag1, flag2;

    public Dictionary<Vector2Int, CellTest> _cells = new Dictionary<Vector2Int, CellTest>();



    private void Awake()
    {
        flag1 = Instantiate(flag);
        flag2 = Instantiate(flag);

        flag1.gameObject.SetActive(false);
        flag2.gameObject.SetActive(false);

        _modeTxt.text = selectionMode.ToString();
    }

    public void InitGrid()
    {
        this.ClearSelections();
        if (_cells != null || _cells.Count != 0)
        {
            foreach (var cell in _cells.Values)
            {
                Destroy(cell.gameObject);
            }
            _cells.Clear();
        }

        grid = Extend(RandomGrid());
        string gridText = string.Empty;
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                gridText += grid[i, j].ToString() + " ";
            }
            gridText += "\n";
        }
        _numPathTxt.text = "Path: ...";
        _dfs = new DFS(grid);
    }

    public void SelectCell(CellTest cell)
    {
        switch (selectionMode)
        {
            case SelectionMode.ChangeValue:
                grid[cell.Pos.x, cell.Pos.y] = -1;
                cell.Value = -1;
                break;
            case SelectionMode.SelectNode:
                {
                    if (this.cell1 == null)
                    {
                        this.cell1 = cell;
                        flag1.transform.SetParent(this.cell1.transform);
                        flag1.gameObject.SetActive(true);
                        flag2.gameObject.SetActive(false);
                        flag1.transform.localPosition = Vector3.zero;
                    }
                    else if (this.cell2 == null)
                    {
                        this.cell2 = cell;
                        flag2.gameObject.SetActive(true);
                        flag2.transform.SetParent(this.cell2.transform);
                        flag2.transform.localPosition = Vector3.zero;
                    }
                }
                break;
        }
    }

    public void ClearSelections()
    {
        flag1.transform.SetParent(null);
        flag2.transform.SetParent(null);

        flag1.gameObject.SetActive(false);
        flag2.gameObject.SetActive(false);

        cell1 = null;
        cell2 = null;
    }

    public void FindAllPath()
    {
        var paths = _dfs.FindAllPaths(cell1.Pos, cell2.Pos);
        if (paths != null) _numPathTxt.text = "Paths: " + paths.Count.ToString();
        StartCoroutine(DrawPath(paths));
    }

    public void ToggleMode()
    {
        if (selectionMode == SelectionMode.ChangeValue)
        {
            selectionMode = SelectionMode.SelectNode;
        }
        else
        {
            selectionMode = SelectionMode.ChangeValue;
        }
        _modeTxt.text = selectionMode.ToString();
    }

    IEnumerator DrawPath(List<List<Vector2Int>> paths)
    {
        foreach (var path in paths)
        {
            foreach (var p in path)
            {
                _cells[p].Visited = true;
                yield return new WaitForSeconds(drawSpeed);
            }
            foreach (var p in path)
            {
                _cells[p].Visited = false;
            }
        }
    }

    public void ResetPath()
    {
        foreach (var cell in _cells)
        {
            cell.Value.Visited = false;
        }

        cell1 = null;
        cell2 = null;
    }

    public void GenerateGrid()
    {
        Vector2 CellSize = prefab.GetComponent<SpriteRenderer>().bounds.size;
        Vector3 firstPosition = Vector3.zero;
        firstPosition.x = (CellSize.x + CellOffset.x) * (sizeX - 1);
        firstPosition.y = (CellSize.y + CellOffset.y) * (sizeY - 1);
        firstPosition *= -(0.5f);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector3 cellPosition = firstPosition + new Vector3(x * (CellSize.x + CellOffset.x), y * (CellSize.y + CellOffset.y), 0);
                CellTest cell = Instantiate(prefab, transform);
                cell.Pos = new Vector2Int(x, y);
                cell.Visited = false;
                cell.Value = grid[x, y];
                cell.transform.localPosition = cellPosition;
                _cells.Add(cell.Pos, cell);
            }
        }
    }

    int[,] RandomGrid()
    {
        int[,] grid = new int[sizeX, sizeY];
        for (int i = 0; i < sizeX - 2; i++)
        {
            for (int j = 0; j < sizeY - 2; j++)
            {
                grid[i, j] = UnityEngine.Random.Range(ranMin, ranMax);
            }
        }

        return grid;
    }

    int[,] Extend(int[,] grid)
    {
        Vector2Int newSize = new Vector2Int(sizeX, sizeY);
        int[,] result = new int[newSize.x, newSize.y];

        for (int i = 0; i < newSize.x; i++)
        {
            for (int j = 0; j < newSize.y; j++)
            {
                if (i == 0 || i == newSize.x - 1 || j == 0 || j == newSize.y - 1)
                {
                    result[i, j] = -1;
                }
                else
                {
                    result[i, j] = grid[i - 1, j - 1];
                }
            }
        }

        return result;
    }
}
