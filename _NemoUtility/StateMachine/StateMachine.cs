using UnityEngine;

namespace NemoUtility
{
    public class StateMachine<T>
    {
        public StateBase<T> CurrentState;
        private T _owner;

        public StateMachine(T owner)
        {
            _owner = owner;
        }

        public void ChangeState(StateBase<T> newState)
        {
            CurrentState?.ExitState(_owner);
            CurrentState = newState;
            CurrentState?.EnterState(_owner);
        }

        public void Update()
        {
            CurrentState?.UpdateState(_owner);
        }
    }

}