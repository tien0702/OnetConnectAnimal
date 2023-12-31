using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameController : SingletonBehaviour<GameController>
{
    #region Events
    public enum GameEvent { OnWinGame, OnLoseGame };
    public ObserverEvents<GameEvent, GameController> Events { private set; get; }
        = new ObserverEvents<GameEvent, GameController>();
    #endregion

    public enum GameState { InProcess, Pause, WinState, GameOverState };

    public GameState State { get; private set; } = GameState.InProcess;

    [SerializeField] EffectController _winPanel, _gameOverPanel;
    [SerializeField] TextMeshProUGUI _winScore, _gameOverScore;

    GameLevelDAL _gameLevelDAL;

    protected override void Awake()
    {
        var buttons = GameObject.FindObjectsOfType<Button>();
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFX("click");
            });
        }

        _gameLevelDAL = new GameLevelDAL();
    }

    private void Start()
    {
        GameLevelInfo levelInfo = _gameLevelDAL.GetGameLevelInfo(GlobalData.GameLevel);
        HUDLayer.Instance.Timer.Init(levelInfo.Time);
        HUDLayer.Instance.Timer.Events
            .RegisterEvent(TimerController.TimerEvent.OnTimerEnd, OnTimeEnded);

        HUDLayer.Instance.Hint.Init(levelInfo.Hint);

        var grid = GameObject.FindAnyObjectByType<GridAnimalController>();
        grid.Events.RegisterEvent(GridAnimalController.GridAnimalEvent.OnClear, OnClearAnimals);

        var gridInfo = User.Instance.Data.Grid;

        var result = grid.GridExtend(gridInfo.CellInfos, gridInfo.GridSize);
        gridInfo.GridSize = result.Item1;
        gridInfo.CellInfos = result.Item2;
        grid.Init(gridInfo);
    }

    public void PauseGame()
    {
        State = GameState.Pause;
        HUDLayer.Instance.Timer.Pause();
    }

    public void ResumeGame()
    {
        State = GameState.InProcess;
        HUDLayer.Instance.Timer.Resume();
    }

    void OnTimeEnded(double seconds)
    {
        _gameOverPanel.gameObject.SetActive(true);
        _gameOverPanel.ShowEffects();
        _gameOverScore.text
            = "Score: " + HUDLayer.Instance.ScoreController.Score.ToString();
        State = GameState.GameOverState;
    }

    void OnClearAnimals(int id)
    {
        _winPanel.gameObject.SetActive(true);
        _winPanel.ShowEffects();
        _winScore.text
            = "Score: " + HUDLayer.Instance.ScoreController.Score.ToString();
        State = GameState.WinState;
    }
}
