using System.Collections.Generic;

namespace TT
{
    public class ActionNode : ComponentController
    {
        #region Leaf Nodes
        protected LinkedList<ICondition> _conditions = new LinkedList<ICondition>();
        protected LinkedList<IResetCondition> _resetConditions = new LinkedList<IResetCondition>();

        protected LinkedList<IHandle> _handles = new LinkedList<IHandle>();
        protected LinkedList<IClearHandle> _clearHandles = new LinkedList<IClearHandle>();
        #endregion

        protected int _actionHandling;
        public virtual bool IsSuitable()
        {
            foreach (ICondition condition in _conditions)
            {
                if (!condition.IsSuitable) return false;
            }
            ResetConditions();
            return true;
        }

        protected virtual void ResetConditions()
        {
            foreach (IResetCondition reset in _resetConditions)
            {
                reset.ResetCondition();
            }
        }

        public bool IsHandling() => _actionHandling > 0;

        public virtual void ActionHandle()
        {
            _actionHandling = _handles.Count;
            foreach (IHandle handle in _handles)
            {
                handle.Handle();
            }
        }

        protected virtual void OnEndHandle(IHandle handle)
        {
            --_actionHandling;
            if (_actionHandling == 0)
            {
                ClearHandles();
            }
        }

        protected virtual void ClearHandles()
        {
            foreach (IClearHandle clear in _clearHandles)
            {
                clear.ClearHandle();
            }
        }

        protected override void Clear()
        {
            base.Clear();
            _conditions.Clear();
            _resetConditions.Clear();
            _handles.Clear();
            _clearHandles.Clear();
        }

        protected override void OnAddComponent(object component)
        {
            if (component is ICondition)
            {
                _conditions.AddLast((ICondition)component);
            }

            if (component is IResetCondition)
            {
                _resetConditions.AddLast((IResetCondition)component);
            }

            if (component is IHandle)
            {
                IHandle handle = (IHandle)component;
                _handles.AddLast(handle);
                handle.OnEndHandle = OnEndHandle;
            }

            if (component is IClearHandle)
            {
                _clearHandles.AddLast((IClearHandle)component);
            }
        }
    }
}
