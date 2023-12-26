using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class Observer : Singleton<Observer>
    {
        Dictionary<string, HashSet<Action<object>>> observerTopics = new Dictionary<string, HashSet<Action<object>>>();

        public void AddObserver(string topicName, Action<object> observer)
        {
            HashSet<Action<object>> observers = CreateObserverTopic(topicName);
            observers.Add(observer);
        }

        public void RemoveObserver(string topicName, Action<object> observer)
        {
            HashSet<Action<object>> observers = CreateObserverTopic(topicName);
            observers.Remove(observer);
        }

        public void RemoveObserverTopic(string topicName)
        {
            if (observerTopics.ContainsKey(topicName))
                observerTopics.Remove(topicName);
        }

        public void Notify(string topicName)
        {
            HashSet<Action<object>> observers = CreateObserverTopic(topicName);
            foreach (Action<object> observer in observers)
                observer(null);
        }

        public void NotifyWithData(string topicName, object data)
        {
            HashSet<Action<object>> observers = CreateObserverTopic(topicName);
            foreach (Action<object> observer in observers)
                observer(data);
        }

        protected HashSet<Action<object>> CreateObserverTopic(string topicName)
        {
            if (!observerTopics.ContainsKey(topicName))
                observerTopics.Add(topicName, new HashSet<Action<object>>());
            return observerTopics[topicName];
        }
    }
}