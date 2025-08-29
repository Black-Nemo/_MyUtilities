using UnityEngine;

namespace NemoUtility
{
    public class LookCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.transform.rotation * Vector3.forward, Camera.main.transform.transform.rotation * Vector3.up);
            //transform.LookAt(Camera.main.gameObject.transform);
        }
    }

}