using System;
using UnityEngine;

namespace TT
{
    public class TTMonoBehaviour : MonoBehaviour
    {
        public float _time = 0.05f;
        protected int _id;
        public LeanTweenType _leanTweenType;

        public virtual void MoveToPosition(Vector3 posTarget, Action callbackOnComplete = null)
        {
            LeanTween.move(gameObject, posTarget, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete);
        }

        public virtual void MoveToPositionUpdate(Vector3 posMoveTo, Action callbackOnComplete = null)
        {
            LeanTween.cancel(_id);
            _id = LeanTween.move(gameObject, posMoveTo, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete).id;
        }

        public virtual void MoveToLocalPosition(Vector3 posTarget, Action callbackOnComplete = null)
        {
            LeanTween.moveLocal(gameObject, posTarget, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete);
        }

        public virtual void MoveToLocalPositionUpdate(Vector3 posMoveTo, Action callbackOnComplete = null)
        {
            LeanTween.cancel(_id);
            _id = LeanTween.moveLocal(gameObject, posMoveTo, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete).id;
        }

        public virtual void ScalceTo(Vector3 targetValue, Action callbackOnComplete = null)
        {
            LeanTween.scale(gameObject, targetValue, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete);
        }

        public virtual void ScalceUpdate(Vector3 targetValue, Action callbackOnComplete = null)
        {
            LeanTween.cancel(_id);
            _id = LeanTween.scale(gameObject, targetValue, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete).id;
        }

        public virtual void RotateTo(Vector3 angleTarget, Action callbackOnComplete = null)
        {
            LeanTween.cancel(_id);
            _id = LeanTween.rotate(gameObject, angleTarget, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete).id;
        }

        public virtual void RotateLocalTo(Vector3 angleTarget, Action callbackOnComplete = null)
        {
            LeanTween.cancel(_id);
            _id = LeanTween.rotateLocal(gameObject, angleTarget, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete).id;
        }

        public virtual void UpdateValue(float from, float to, Action<float> callbackOnUpdate = null, Action callbackOnComplete = null)
        {
            LeanTween.cancel(_id);
            _id = LeanTween.value(gameObject, callbackOnUpdate, from, to, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete).id;
        }

        public virtual void UpdateValue(Vector2 from, Vector2 to, Action<Vector2> callbackOnUpdate = null, Action callbackOnComplete = null)
        {
            LeanTween.cancel(_id);
            _id = LeanTween.value(gameObject, callbackOnUpdate, from, to, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete).id;
        }

        public virtual void UpdateValue(Color from, Color to, Action<Color> callbackOnUpdate = null, Action callbackOnComplete = null)
        {
            LeanTween.cancel(_id);
            _id = LeanTween.value(gameObject, callbackOnUpdate, from, to, _time).setEase(_leanTweenType).setOnComplete(callbackOnComplete).id;
        }
    }
}
