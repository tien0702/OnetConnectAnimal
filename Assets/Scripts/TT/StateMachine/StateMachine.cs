using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TT
{
    public class StateMachine : ComponentController
    {
        #region Events
        public enum StateMachineEventType { OnChangeState };
        ObserverEvents<StateMachineEventType, StateController> _events = new ObserverEvents<StateMachineEventType, StateController>();
        public ObserverEvents<StateMachineEventType, StateController> Events => _events;
        #endregion

        Dictionary<int, StateController> _states = new Dictionary<int, StateController>();

        [SerializeField] int _currentState;
        public int CurrentState => _currentState;
        public StateController[] States => _states.Values.ToArray();

        protected virtual void Start()
        {
            _states[_currentState].OnEnter();
        }

        protected virtual void Update()
        {
            int state = _states[_currentState].OnUpdate(Time.deltaTime);
            if (state.Equals(_currentState)) return;
            _states[_currentState].OnExit();
            _currentState = state;
            _events.Notify(StateMachineEventType.OnChangeState, _states[_currentState]);
            _states[_currentState].OnEnter();
        }

        public override bool Init(string data)
        {
            this.Clear();

            var nodeTypes = NodeUtils.GetNodeTypes(data);

            JSONNode jsonData = JSONObject.Parse(data);

            JSONArray jsonArray = jsonData["data"].AsArray;

            string[] datas = new string[jsonArray.Count];

            for (int i = 0; i < jsonArray.Count; ++i)
            {
                datas[i] = jsonArray[i]["data"].ToString();
            }

            for (int i = 0; i < nodeTypes.Length; ++i)
            {
                var component = ComponentHelper.AddComponent(this.gameObject, nodeTypes[i], OnAddComponent);
                if (component is StateController)
                {
                    var stateController = (StateController)component;
                    stateController.Init(datas[i]);
                }
            }
            return true;
        }

        protected override void OnAddComponent(object component)
        {
            if (component is StateController)
            {
                StateController state = (StateController)component;
                if (_states.Count == 0)
                {
                    _currentState = state.Info.StateHash.State;
                }
                if (!_states.ContainsKey(state.Info.StateHash.State))
                {
                    _states.Add(state.Info.StateHash.State, state);
                }
                else
                {
                    _states[state.Info.StateHash.State] = state;
                }
            }
        }

        public StateController GetState(int stateHash)
        {
            if (!_states.ContainsKey(stateHash))
            {
                return null;
            }
            return _states[stateHash];
        }

        public StateController GetStateByName(string stateName)
        {
            foreach (var state in _states)
            {
                if (state.Value.Info.StateName.Equals(stateName))
                {
                    return state.Value;
                }
            }
            return null;
        }
    }
}