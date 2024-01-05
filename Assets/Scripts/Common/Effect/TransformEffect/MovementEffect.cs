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
                MoveBy(_toPosition, () => { callbackOnComplete(this); });
                break;
            case TransformMode.Global:
                transform.position = _originPostion;
                MoveTo(_toPosition, () => { callbackOnComplete(this); });
                break;
        }
    }

    public void HideEffect(Action<IEffect> callbackOnComplete)
    {
        LeanTween.cancel(this._id);
        switch (_mode)
        {
            case TransformMode.Local:
                MoveBy(_originPostion, () => { callbackOnComplete(this); });
                break;
            case TransformMode.Global:
                MoveTo(_originPostion, () => { callbackOnComplete(this); });
                break;
        }
    }
}
