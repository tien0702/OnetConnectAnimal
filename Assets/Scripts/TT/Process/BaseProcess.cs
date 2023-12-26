using System;

namespace TT
{
    public abstract class BaseProcess : TTMonoBehaviour
    {
        protected float minValue = 0;
        protected float currentValue;
        protected float maxValue;

        public virtual float MinValue
        {
            set
            {
                minValue = value;
                if (maxValue <= minValue) MaxValue = minValue * 2;
                if (currentValue < minValue) CurrentValue = minValue;
            }
            get => minValue;
        }

        public virtual float CurrentValue
        {
            set
            {
                currentValue = value;
                if (currentValue < minValue) currentValue = minValue;
                if (currentValue > maxValue) currentValue = maxValue;
            }
            get => currentValue;
        }

        public virtual float MaxValue
        {
            set
            {
                maxValue = value;
                if (currentValue > maxValue) CurrentValue = maxValue;
            }
            get => maxValue;
        }

        public virtual void LinearlyValue(float newValue, Action callbackOnComplete)
        {
            float preValue = currentValue;
            CurrentValue = newValue;
            this.UpdateValue(preValue, currentValue, OnChangeValue, callbackOnComplete);
        }

        protected abstract void OnChangeValue(float value);
    }
}
