using SimpleJSON;
using UnityEngine;

namespace TT
{
    public abstract class ComponentController : MonoBehaviour
    {
        protected Component[] _components = null;

        protected virtual void OnDestroy()
        {
            if (_components == null) return;
            foreach (var component in _components)
            {
                if (component != null)
                {
                    Destroy(component);
                }
            }
        }

        public virtual bool Init(string data)
        {
            Clear();
            NodeType[] componentTypes = NodeUtils.GetNodeTypes(data);
            _components = ComponentHelper.AddComponents(this.gameObject, componentTypes, OnAddComponent);
            return true;
        }

        protected virtual void Clear()
        {
            if (_components == null) { return; }
            foreach (var component in _components)
            {
                Destroy(component);
            }
        }

        protected abstract void OnAddComponent(object component);
    }
}
