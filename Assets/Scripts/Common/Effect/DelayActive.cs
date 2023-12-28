using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayActive : MonoBehaviour, IEffect
{
    [SerializeField] float _delayActiveTrue, _delayActiveFalse;
    public void ShowEffect(Action<IEffect> callbackOnComplete)
    {
        LeanTween.delayedCall(_delayActiveTrue, () => { gameObject.SetActive(true); });
    }

    public void HideEffect(Action<IEffect> callbackOnComplete)
    {
        LeanTween.delayedCall(_delayActiveFalse, () => { gameObject.SetActive(false); });
    }
}
