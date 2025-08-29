using System.Collections;
using UnityEngine;
using TMPro;

namespace NemoUtility
{
    public class FpsShow : MonoBehaviour
    {
        public TextMeshProUGUI fpsText;
        private float sequency = 1f;
        public float deltaTime;

        private float _fps;
        private void Start()
        {
            StartCoroutine(FpsEnumator());
        }

        void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            _fps = Mathf.Ceil(fps);
        }


        private IEnumerator FpsEnumator()
        {
            while (true)
            {
                fpsText.text = _fps.ToString();
                yield return new WaitForSeconds(sequency);
            }
        }
    }
}