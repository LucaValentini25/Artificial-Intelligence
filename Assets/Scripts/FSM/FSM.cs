using System.Collections.Generic;

namespace FSM
{
    public class Fsm 
    {
        Dictionary<NpcStates, IState> _states = new Dictionary<NpcStates, IState>();

        IState _actualState;

        public void CreateState(NpcStates name, IState state)
        {
            if(_actualState== null) _actualState = state;
            if (!_states.ContainsKey(name))
                _states.Add(name, state);
        }

        public void Execute()
        {
            _actualState.OnUpdate();
        }

        public void ChangeState(NpcStates name)
        {
            if (_states.ContainsKey(name))
            {
                if (_actualState != null)
                    _actualState.OnExit();

                _actualState = _states[name];
                _actualState.OnEnter();
            }
        }

        public bool IsActualState(NpcStates name)
        {
            if(_states.ContainsKey(name))
                return _actualState == _states[name];
            else return false;
        }
    }

    public enum NpcStates
    {
        Idle,
        Hide,
        Chase,
        Attack
    }
}