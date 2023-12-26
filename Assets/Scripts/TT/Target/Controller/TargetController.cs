using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class TargetController : MonoBehaviour
    {
        protected static List<TargetController> targets = new List<TargetController>();

        public static Transform FindTarget(FilterTargetController filter)
        {
            if (filter == null) return null;
            TargetController bestTarget = null;

            foreach (TargetController target in targets)
            {
                if (!filter.CheckTarget(target.transform)) continue;

                if (bestTarget == null || filter.CheckBestTarget(bestTarget.transform, target.transform))
                {
                    bestTarget = target;
                }
            }

            return bestTarget ? bestTarget.transform : null;
        }

        public static List<Transform> FindTargets(FilterTargetController filter, int numRequire)
        {
            if (targets == null || targets.Count == 0 || filter == null || numRequire == 0)
                return new List<Transform>();

            List<Transform> result = new List<Transform>();
            if (numRequire == 1)
            {
                Transform target = FindTarget(filter);

                if (target != null) result.Add(target);
                return result;
            }

            foreach (TargetController target in targets)
            {
                if (!filter.CheckTarget(target.transform)) continue;
            }

            return result;
        }

        protected virtual void Awake()
        {
            if (targets == null) targets = new List<TargetController>();
            if (targets.Contains(this)) return;
            targets.Add(this);
        }

        protected virtual void OnDestroy()
        {
            if (targets != null) targets.Remove(this);
        }

        protected virtual void OnDisable()
        {
            if (targets != null) targets.Remove(this);
        }

        protected virtual void OnEnable()
        {
            if (targets.Contains(this)) return;
            targets.Add(this);
        }
    }
}
