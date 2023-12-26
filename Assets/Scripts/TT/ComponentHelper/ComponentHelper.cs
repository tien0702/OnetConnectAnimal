using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TT
{
    public class ComponentHelper
    {
        public static Component AddComponent(GameObject obj, NodeType nodeType, Action<object> callbackOnComplete = null)
        {
            object component = null;
            bool isComponent = false;
            if (nodeType.Type.IsSubclassOf(typeof(Component)))
            {
                component = obj.AddComponent(nodeType.Type);
                isComponent = true;
            }
            else
            {
                component = (object)Activator.CreateInstance(nodeType.Type);
            }

            if (nodeType.NodeData != null)
            {
                IInfo info = (IInfo)component;
                if (info != null)
                {
                    info.SetInfo(nodeType.NodeData);
                }
            }

            if (callbackOnComplete != null)
            {
                callbackOnComplete(component);
            }

            return (isComponent) ? (component as Component) : (null);
        }

        public static Component[] AddComponents(GameObject obj, NodeType[] componentTypes, Action<object> callbackOnComplete = null)
        {
            LinkedList<Component> components = new LinkedList<Component>();
            for (int i = 0; i < componentTypes.Length; i++)
            {
                Component component = ComponentHelper.AddComponent(obj, componentTypes[i], callbackOnComplete);
                if (component != null)
                {
                    components.AddLast(component);
                }
            }
            return components.ToArray();
        }
    }
}
