using System;
using System.Collections;
using System.Collections.Generic;
using TT;
using UnityEngine;

public class RotationEffect : TTMonoBehaviour, IEffect
{
    [SerializeField] TransformMode _mode;
    [SerializeField] Vector3 _toRotation;
    Vector3 _originRotation;

    private void Awake()
    {
        switch (_mode)
        {
            case TransformMode.Local:
                _originRotation = transform.localEulerAngles;
                break;
            case TransformMode.Global:
                _originRotation = transform.eulerAngles;
                break;
        }
    }

    public void ShowEffect(Action<IEffect> callbackOnComplete)
    {
        switch (_mode)
        {
            case TransformMode.Local:
                transform.localEulerAngles = _originRotation;
                this.RotateLocalTo(_toRotation, () => { callbackOnComplete(this); });
                break;
            case TransformMode.Global:
                transform.eulerAngles = _originRotation;
                this.RotateTo(_toRotation, () => { callbackOnComplete(this); });
                break;
        }
    }

    public void HideEffect(Action<IEffect> callbackOnComplete)
    {
        LeanTween.cancel(this._id);
        switch (_mode)
        {
            case TransformMode.Local:
                this.RotateLocalTo(_originRotation, () => { callbackOnComplete(this); });
                break;
            case TransformMode.Global:
                this.RotateTo(_originRotation, () => { callbackOnComplete(this); });
                break;
        }
    }
}
