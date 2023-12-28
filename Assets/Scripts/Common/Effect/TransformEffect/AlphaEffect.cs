using System;
using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AlphaEffect : TTMonoBehaviour, IEffect
{
    [SerializeField] float _toAlpha;
    [SerializeField] float _originAlpha;

    public void ShowEffect(Action<IEffect> callbackOnComplete)
    {
        LeanTween.alpha(gameObject, _toAlpha, _time)
            .setEase(_leanTweenType)
            .setOnComplete(() => { callbackOnComplete(this); });
    }

    public void HideEffect(Action<IEffect> callbackOnComplete)
    {
        LeanTween.cancel(this._id);
        LeanTween.alpha(gameObject, _originAlpha, _time)
            .setEase(_leanTweenType) 
            .setOnComplete(() => { callbackOnComplete(this); });
    }
}
