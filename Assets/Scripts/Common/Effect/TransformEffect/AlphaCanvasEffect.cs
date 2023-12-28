using System;
using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class AlphaCanvasEffect : TTMonoBehaviour, IEffect
{
    [SerializeField] float _toAlpha;
    [SerializeField] float _originAlpha;

    CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowEffect(Action<IEffect> callbackOnComplete)
    {
        LeanTween.alphaCanvas(_canvasGroup, _toAlpha, _time)
            .setEase(_leanTweenType)
            .setOnComplete(() => { callbackOnComplete(this); });
    }

    public void HideEffect(Action<IEffect> callbackOnComplete)
    {
        LeanTween.cancel(this._id);
        LeanTween.alphaCanvas(_canvasGroup, _originAlpha, _time)
            .setEase(_leanTweenType)
            .setOnComplete(() => { callbackOnComplete(this); });
    }
}
