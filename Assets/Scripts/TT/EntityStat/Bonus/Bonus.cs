using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    [System.Serializable]
    public class Bonus
    {
        [SerializeField] protected BonusInfo info;
        public BonusInfo Info
        {
            get { return info; }
            set
            {
                info = value;
                timeBonus = info.Duration;
            }
        }

        protected float timeBonus;

        public bool IsEndBonus => (timeBonus <= 0) && info.Duration != 0f;

        public Bonus(BonusInfo info)
        {
            this.info = info;
            timeBonus = info.Duration;
        }

        public void CalculateDuration(float dt)
        {
            timeBonus -= dt;
        }

        public void Reset()
        {
            timeBonus = info.Duration;
        }
    }
}
