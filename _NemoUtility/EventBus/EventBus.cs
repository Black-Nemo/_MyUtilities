using System;

namespace NemoUtility
{
    public class EventBus<T> where T : IEvent
    {
        private static readonly object _lock = new object();

        public static EventBus<T> Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_eventBus == null)
                    {
                        _eventBus = new EventBus<T>();
                    }
                    return _eventBus;
                }
            }
        }

        private static EventBus<T> _eventBus;


        private Action<T> _action;
        private Action _actionNoArgs;

        //Args
        public void Subscribe(Action<T> onEvent)
        {
            _action += onEvent;
        }
        public void UnSubscribe(Action<T> onEvent)
        {
            _action -= onEvent;
        }
        //NoArgs
        public void Subscribe(Action onEvent)
        {
            _actionNoArgs += onEvent;
        }
        public void UnSubscribe(Action onEvent)
        {
            _actionNoArgs -= onEvent;
        }

        public void Publish(T onEvent)
        {
            _action?.Invoke(onEvent);
            _actionNoArgs?.Invoke();
        }
    }
}
