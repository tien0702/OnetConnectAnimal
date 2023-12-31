using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TT;
using System.Linq;

public class SelectionController : MonoBehaviour, IOnTouchEnded
{
    [SerializeField] string _selectSoundName;
    public static readonly Vector2Int NotSelect = new Vector2Int(-1, -1);
    static Vector2Int _animalSelected = NotSelect;
    static Transform[] _selections = null;
    static bool _lockSelect = false;

    private void OnDestroy()
    {
        _selections = null;
        _animalSelected = NotSelect;
    }

    public static void Select(Transform target, int index)
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
        if (index == 0)
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
    CellController cell;

    GridAnimalController grid;


    private void Awake()
    {
        cell = GetComponent<CellController>();
        grid = GetComponentInParent<GridAnimalController>();
    }

    public void OnTouchEnded()
    {
        if (_lockSelect) return;
        if(cell.ID == -1) return;
        if (GameController.Instance.State != GameController.GameState.InProcess) return;

        //AudioManager.Instance.PlaySFX(_selectSoundName);

        if (_animalSelected.Equals(NotSelect) || 
            (grid.GetCell(_animalSelected).ID != grid.GetCell(cell.CellPos).ID))
        {
            _animalSelected = cell.CellPos;
            Select(transform, 0);
        }
        else if (!(_animalSelected.x == cell.CellPos.x
            && _animalSelected.y == cell.CellPos.y))
        {
            Vector2Int[] path = grid.FindPath(_animalSelected, cell.CellPos);
            if (path == null || (grid.CountTurns(path) > 2))
            {
                _animalSelected = cell.CellPos;
                Select(transform, 0);
            }
            else
            {
                Select(transform, 1);
                _lockSelect = true;
                grid.DrawLine(grid.ConvertPathToPoints(path), () =>
                {
                    grid.RemoveAt(_animalSelected);
                    grid.RemoveAt(cell.CellPos);
                    _selections[0].gameObject.SetActive(false);
                    _selections[1].gameObject.SetActive(false);
                    _animalSelected = NotSelect;
                    _lockSelect = false;
                });
            }
        }
    }
}
