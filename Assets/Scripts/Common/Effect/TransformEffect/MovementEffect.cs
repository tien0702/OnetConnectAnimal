using System;
using TT;
using UnityEngine;

public class MovementEffect : TTMonoBehaviour, IEffect
{
    [SerializeField] TransformMode _mode;
    [SerializeField] Vector3 _toPosition;
    Vector3 _originPostion;

    private void Awake()
    {

        switch (_mode)
        {
            case TransformMode.Local:
                _originPostion = transform.localPosition;
                break;
            case TransformMode.Global:
                _originPostion = transform.position;
                break;
        }
    }

    public void ShowEffect(Action<IEffect> callbackOnComplete)
    {

        switch (_mode)
        {
            case TransformMode.Local:
                transform.localPosition = _originPostion;
                MoveToLocalPosition(_toPosition, () => { callbackOnComplete(this); });
                break;
            case TransformMode.Global:
                transform.position = _originPostion;
                MoveToPosition(_toPosition, () => { callbackOnComplete(this); });
                break;
        }
    }

    public void HideEffect(Action<IEffect> callbackOnComplete)
    {
        LeanTween.cancel(this._id);
        switch (_mode)
        {
            case TransformMode.Local:
                MoveToLocalPosition(_originPostion, () => { callbackOnComplete(this); });
                break;
            case TransformMode.Global:
                MoveToPosition(_originPostion, () => { callbackOnComplete(this); });
                break;
        }
    }
}
