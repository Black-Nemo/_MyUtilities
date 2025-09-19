namespace NemoUtility
{
    public abstract class StateBase<T>
    {
        public StateBase(StateMachine<T> stateMachine)
        {
            StateMachine = stateMachine;
        }
        public StateMachine<T> StateMachine;


        public abstract void EnterState(T obj);
        public abstract void UpdateState(T obj);
        public abstract void ExitState(T obj);

        public virtual void Init()
        {

        }
    }
}
