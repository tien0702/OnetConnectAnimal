using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;

public class GameController : SingletonBehaviour<GameController>
{
    #region Events
    public enum GameEvent { OnWinGame, OnLoseGame };
    public ObserverEvents<GameEvent, GameController> Events { private set; get; } 
        = new ObserverEvents<GameEvent, GameController>();
    #endregion

    private void Start()
    {
        HUDLayer.Instance.Timer.Events
            .RegisterEvent(TimerController.TimerEvent.OnTimerEnd, OnTimeEnded);

        var grid = GameObject.FindAnyObjectByType<GridController>();

        grid.Init(User.Instance.Data.Grid);

        CellInfo[] cells = new CellInfo[(int)(User.Instance.Data.Grid.GridSize.x * User.Instance.Data.Grid.GridSize.y)];

        var cell = grid.GetCell(1, 5);
        Debug.Log(cell.gameObject.name);
    }

    void OnTimeEnded(double seconds)
    {

    }
}
