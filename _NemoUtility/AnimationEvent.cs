using System;
using UnityEngine;

namespace NemoUtility
{
    public class AnimationEvent : MonoBehaviour
    {
        public Action<int> OnAnimationEvent;

        private int _counter;
        public void Invoke()
        {
            OnAnimationEvent?.Invoke(_counter);
            _counter++;
        }

        public void EndAninmation()
        {
            _counter = 0;
        }
    }
}