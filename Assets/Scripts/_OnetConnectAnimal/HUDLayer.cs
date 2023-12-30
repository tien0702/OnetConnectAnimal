using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TT;

public class HUDLayer : SingletonBehaviour<HUDLayer>
{
    [SerializeField] protected TextMeshProUGUI _levelTxt;
    [SerializeField] protected TimerController _timerController;
    [SerializeField] protected ScoreController _scoreController;
    [SerializeField] protected HintController _hintController;

    public TimerController Timer => _timerController;
    public ScoreController ScoreController => _scoreController;
    public HintController Hint => _hintController;

    protected override void Awake()
    {
        _scoreController = GetComponentInChildren<ScoreController>();
    }

    private void Start()
    {
        _levelTxt.text = "Level " + GlobalData.GameLevel.ToString();
    }
}
