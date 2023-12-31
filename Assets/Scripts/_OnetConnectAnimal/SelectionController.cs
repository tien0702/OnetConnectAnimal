using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TT;
using System.Linq;

public class SelectionController : MonoBehaviour, IOnTouchEnded
{
    #region Selection Manager
    [SerializeField] string _selectSoundName;
    public static readonly Vector2Int NotSelect = new Vector2Int(-1, -1);
    static Vector2Int _animalSelected = NotSelect;
    static Transform[] _selections = null;
    static bool _lockSelect = false;
    public static void ShowSelect(Transform target, SelectionType selectionType)
    {
        if (_selections == null)
        {
            Transform selectionPrefab = Resources.Load<GameObject>("Prefabs/Selection").transform;
            _selections = new Transform[2];
            for (int i = 0; i < 2; ++i)
            {
                _selections[i] = Instantiate(selectionPrefab) as Transform;
                _selections[i].gameObject.SetActive(false);
                _selections[i].gameObject.name = string.Format("[Selection: {0}]", (i + 1).ToString());
            }
        }
        if (selectionType == SelectionType.FirstSelect)
        {
            _selections[1].gameObject.SetActive(false);
            _selections[0].gameObject.SetActive(true);
            _selections[0].SetParent(target);
            _selections[0].localPosition = Vector3.zero;
        }
        else
        {
            _selections[1].gameObject.SetActive(true);
            _selections[1].SetParent(target);
            _selections[1].localPosition = Vector3.zero;
        }
    }

    public static void Select(CellController cell, GridAnimalController grid)
    {
        if (_animalSelected.Equals(NotSelect) ||
            (grid.GetCell(_animalSelected).ID != grid.GetCell(cell.CellPos).ID))
        {
            _animalSelected = cell.CellPos;
            ShowSelect(cell.transform, SelectionType.FirstSelect);
        }
        else if (!(_animalSelected.x == cell.CellPos.x
            && _animalSelected.y == cell.CellPos.y))
        {
            Vector2Int[] path = grid.FindPath(_animalSelected, cell.CellPos);
            if (path == null || (grid.CountTurns(path) > 2))
            {
                _animalSelected = cell.CellPos;
                ShowSelect(cell.transform, SelectionType.FirstSelect);
            }
            else
            {
                ShowSelect(cell.transform, SelectionType.SecondSelect);
                _lockSelect = true;
                grid.DrawLine(grid.ConvertPathToPoints(path), () =>
                {
                    grid.RemoveAt(_animalSelected);
                    grid.RemoveAt(cell.CellPos);
                    ResetSelection();
                });
            }
        }
    }

    public static void ResetSelection()
    {
        _animalSelected = NotSelect;
        _lockSelect = false;
        for (int i = 0; i < _selections.Length; ++i)
        {
            if (_selections[i] != null)
            {
                _selections[i].gameObject.SetActive(false);
            }
        }
    }

    #endregion

    public enum SelectionType { FirstSelect = 0, SecondSelect = 1 }

    CellController _cell;

    GridAnimalController _grid;

    private void OnDestroy()
    {
        _selections = null;
        _animalSelected = NotSelect;
    }
    private void Awake()
    {
        _cell = GetComponent<CellController>();
        _grid = GetComponentInParent<GridAnimalController>();
    }

    public void OnTouchEnded()
    {
        if (_lockSelect) return;
        if (_cell.ID == -1) return;
        if (GameController.Instance.State != GameController.GameState.InProcess) return;

        //AudioManager.Instance.PlaySFX(_selectSoundName);

        Select(_cell, _grid);
    }
}
