using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public interface IOwn
    {
        void SetOwn(object own);
    }

    public interface IOnCheck
    {
        void OnCheck();
    }

    public class StateInfoHash
    {
        public int State;
        public int Anim;
        public int[] NextStates;

        public StateInfoHash(StateInfo stateInfo)
        {
            State = Animator.StringToHash(stateInfo.StateName);
            Anim = Animator.StringToHash(stateInfo.AnimName);
            NextStates = new int[stateInfo.NextStates.Length];
            for (int i = 0; i < NextStates.Length; ++i)
            {
                NextStates[i] = Animator.StringToHash(stateInfo.NextStates[i]);
            }
        }
    }

    [System.Serializable]
    public class StateInfo
    {
        public string StateName;
        public string AnimName;
        public string[] NextStates;


        private StateInfoHash _stateHash;
        public StateInfoHash StateHash
        {
            get
            {
                if (_stateHash == null)
                {
                    _stateHash = new StateInfoHash(this);
                }
                return _stateHash;
            }
        }
    }

    public class StateController : ActionNode, IInfo
    {
        #region Events
        public enum StateEventType { OnEnter, OnUpdate, OnExit, OnCheck, OnClear }
        ObserverEvents<StateEventType, StateController> _events = new ObserverEvents<StateEventType, StateController>();
        public ObserverEvents<StateEventType, StateController> Events => _events;
        #endregion

        #region Interfaces
        LinkedList<IOnCheck> _onChecks = new LinkedList<IOnCheck>();
        LinkedList<IOwn> _owns = new LinkedList<IOwn>();
        #endregion

        [SerializeField] protected StateInfo _info;
        public StateInfo Info => _info;

        StateMachine _stateMachine;
        Animator _animator;

        protected virtual void Awake()
        {
            _stateMachine = GetComponent<StateMachine>();
            _animator = GetComponentInChildren<Animator>();
        }

        protected virtual void Start()
        {
            foreach(IOwn own in _owns)
            {
                own.SetOwn(this);
            }
        }

        protected override void OnAddComponent(object component)
        {
            base.OnAddComponent(component);
            if (component is IOnCheck)
            {
                _onChecks.AddLast((IOnCheck)component);
            }
            if (component is IOwn)
            {
                _owns.AddLast((IOwn)component);
            }
        }

        public void SetInfo(object data)
        {
            if (data is StateInfo)
            {
                this._info = (StateInfo)data;
            }
        }

        public virtual void OnEnter()
        {
            if(_info.AnimName != null && !_info.AnimName.Equals(string.Empty))
            {
                _animator.Play(_info.StateHash.Anim);
            }

            _events.Notify(StateEventType.OnEnter, this);
            ActionHandle();
        }

        public virtual int OnUpdate(float deltaTime)
        {
            _events.Notify(StateEventType.OnUpdate, this);
            if (IsHandling()) return _info.StateHash.State;
            for (int i = 0; i < _info.NextStates.Length; i++)
            {
                StateController nextState = _stateMachine.GetState(_info.StateHash.NextStates[i]);
                if (nextState != null && nextState.IsSuitable())
                {
                    return _info.StateHash.NextStates[i];
                }
            }

            return _info.StateHash.State;
        }

        public virtual void OnExit()
        {
            _events.Notify(StateEventType.OnExit, this);
        }

        protected override void Clear()
        {
            base.Clear();
            _onChecks.Clear();

            _events.Notify(StateEventType.OnClear, this);
            _events.Clear();
        }

        public override bool IsSuitable()
        {
            foreach (IOnCheck onCheck in _onChecks)
            {
                onCheck.OnCheck();
            }
            _events.Notify(StateEventType.OnCheck, this);
            return base.IsSuitable();
        }
    }
}
