using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TT;

public class HUDLayer : SingletonBehaviour<HUDLayer>
{
    [SerializeField] protected TextMeshProUGUI _levelTxt, _scoreTxt;
    [SerializeField] protected TimerController _timerController;

    public TimerController Timer => _timerController;

    private void Start()
    {
        _levelTxt.text = "Level " + GlobalData.GameLevel.ToString();

        _timerController.Init(1000);
    }
}
