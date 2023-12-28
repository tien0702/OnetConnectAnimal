using System.Collections;
using System.Collections.Generic;
using TMPro;
using TT;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    #region Events
    public enum TimerEvent { OnTimerInit, OnTimerEnd }
    public ObserverEvents<TimerEvent, double> Events
    { private set; get; } = new ObserverEvents<TimerEvent, double>();
    #endregion

    [field: SerializeField] public double Seconds { private set; get; }
    [SerializeField] protected TextMeshProUGUI _timeTxt;

    protected virtual void Awake()
    {
        _timeTxt = GetComponentInChildren<TextMeshProUGUI>();
    }

    public virtual bool Init(double seconds)
    {
        if(seconds < 0) seconds = 0;

        this.Seconds = seconds;

        Events.Notify(TimerEvent.OnTimerInit, seconds);

        return true;
    }

    protected virtual void Update()
    {
        if (Seconds <= 0) return;
        Seconds -= Time.deltaTime;

        if(_timeTxt != null)
            _timeTxt.text = "Time: " + Utilities.ConvertToHH_MM_DD((int)Seconds);

        if(Seconds <= 0)
        {
            Events.Notify(TimerEvent.OnTimerEnd, Seconds);
        }
    }


}
