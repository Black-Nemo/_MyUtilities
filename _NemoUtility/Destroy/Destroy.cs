using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NemoUtility
{
    public class Destroy : MonoBehaviour
    {
        [SerializeField] private float destroyTime;
        void Start()
        {
            Destroy(gameObject, destroyTime);
        }
    }
}

