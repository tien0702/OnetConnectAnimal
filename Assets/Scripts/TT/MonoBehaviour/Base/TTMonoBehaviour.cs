using System;
using UnityEngine;

namespace TT
{
    public class TTMonoBehaviour : MonoBehaviour
    {
        public float _time = 0.05f;
        protected int _id;
        public LeanTweenType _leanTweenType;

        public virtual void MoveTo(Vector3 posTarget, Action callbackOnComplete = null)
        {
            LeanTween.move(gameObject, posTarget, _time)
                .setEase(_leanTweenType)
                .setOnComplete(callbackOnComplete);
        }

        public virtual void MoveBy(Vector3 posTarget, Action callbackOnComplete = null)
        {
            LeanTween.moveLocal(gameObject, posTarget, _time)
                .setEase(_leanTweenType)
                .setOnComplete(callbackOnComplete);
        }

        public virtual void ScalceTo(Vector3 targetValue, Action callbackOnComplete = null)
        {
            LeanTween.scale(gameObject, targetValue, _time)
                .setEase(_leanTweenType)
                .setOnComplete(callbackOnComplete);
        }

        public virtual void ScalceBy(Vector3 targetValue, Action callbackOnComplete = null)
        {
            LeanTween.scale(gameObject, gameObject.transform.localScale + targetValue, _time)
                .setEase(_leanTweenType)
                .setOnComplete(callbackOnComplete);
        }

        public virtual void RotateTo(Vector3 angleTarget, Action callbackOnComplete = null)
        {
            LeanTween.rotate(gameObject, angleTarget, _time)
                .setEase(_leanTweenType)
                .setOnComplete(callbackOnComplete);
        }

        public virtual void RotateBy(Vector3 angleTarget, Action callbackOnComplete = null)
        {
            LeanTween.rotateLocal(gameObject, gameObject.transform.eulerAngles + angleTarget, _time)
                .setEase(_leanTweenType)
                .setOnComplete(callbackOnComplete);
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
