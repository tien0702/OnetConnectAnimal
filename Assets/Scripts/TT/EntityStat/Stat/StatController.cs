using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TT
{
    public class StatController : MonoBehaviour
    {
        protected Dictionary<string, Stat> stats = new Dictionary<string, Stat>();

        EntityController owner;

        protected virtual void Awake()
        {
            owner = GetComponentInParent<EntityController>();
        }

        protected virtual void Update()
        {
            foreach (KeyValuePair<string, Stat> kvp in stats)
            {
                kvp.Value.Update(Time.deltaTime);
            }
        }

        public virtual void AddBonus(Bonus bonus)
        {
            if (!stats.ContainsKey(bonus.Info.StatIDBonus))
            {
                Debug.Log(string.Format("{0} does not exists!", bonus.Info.StatIDBonus));
                return;
            }    
            stats[bonus.Info.StatIDBonus].AddBonus(bonus);
        }

        public virtual void SetStatInfos(StatInfo[] statInfos)
        {
            for (int i = 0; i < statInfos.Length; i++)
            {
                SetStatInfo(statInfos[i]);
            }
        }

        public virtual void SetStatInfo(StatInfo info)
        {
            if (!stats.ContainsKey(info.StatID))
            {
                stats.Add(info.StatID, new Stat());
            }

            stats[info.StatID].Info = info;
        }

        public virtual Stat GetStatByID(string statID)
        {
            if (!stats.ContainsKey(statID)) return null;
            return stats[statID];
        }

        public virtual Stat[] GetStats()
        {
            return stats.Values.ToArray();
        }
    }
}
