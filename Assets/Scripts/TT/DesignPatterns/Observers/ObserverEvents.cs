using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class ObserverEvents<EventType, DataType>
    {
        Dictionary<EventType, HashSet<Action<DataType>>> events;

        public ObserverEvents()
        {
            events = new Dictionary<EventType, HashSet<Action<DataType>>>();
        }

        public virtual void Clear()
        {
            events.Clear();
        }

        public virtual void ClearType(EventType type)
        {
            if (events.ContainsKey(type)) events[type].Clear();
        }

        public virtual void RegisterEvent(EventType eventType, Action<DataType> observer)
        {
            if (!events.ContainsKey(eventType))
            {
                events.Add(eventType, new HashSet<Action<DataType>>());
            }
            events[eventType].Add(observer);
        }

        public virtual void UnRegisterEvent(EventType eventType, Action<DataType> observer)
        {
            if (!events.ContainsKey(eventType)) return;

            events[eventType].Remove(observer);
        }

        public virtual void Notify(EventType eventType, DataType data)
        {
            if (!events.ContainsKey(eventType)) return;

            HashSet<Action<DataType>> observers = events[eventType];
            foreach (Action<DataType> observer in observers)
            {
                if(observer != null) 
                    observer(data);
            }
        }
    }
}
