using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    void ShowEffect(Action<IEffect> callbackOnComplete);
    void HideEffect(Action<IEffect> callbackOnComplete);
}

public class EffectController : MonoBehaviour
{
    IEffect[] _effects;
    int _countEffects;

    private void Awake()
    {
        _effects = GetComponentsInChildren<IEffect>();
    }

    bool toggle = false;

    public void Toggle()
    {
        toggle = !toggle;
        if (toggle)
        {
            ShowEffects();
        }
        else
        {
            HideEffects();
        }
    }

    public void ShowEffects()
    {
        _countEffects = _effects.Length;
        foreach (IEffect effect in _effects)
        {
            effect.ShowEffect(OnShowComplete);
        }
    }

    public void HideEffects()
    {
        _countEffects -= 1;
        foreach (IEffect effect in _effects)
        {
            effect.HideEffect(OnHideComplete);
        }
    }

    void OnShowComplete(IEffect effect)
    {

    }

    void OnHideComplete(IEffect effect)
    {

    }
}
