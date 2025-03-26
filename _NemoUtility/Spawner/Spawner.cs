using UnityEngine;

namespace NemoUtility
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject spawnObject;
        public void Spawn()
        {
            Instantiate(spawnObject, transform.position, transform.rotation);
        }
    }
}