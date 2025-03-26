using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NemoUtility
{
    public class TextWorldCanvas : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public Image Image;
        public float ScaleFactor = 0;
        public float DestroyTime = 1;
        public float Speed = 0;

        public void SetImageSprite(Sprite image)
        {
            Image.sprite = image;
            Image.color = new Color(1, 1, 1, 1);
        }

        private void Start()
        {
            Destroy(gameObject, DestroyTime);
        }

        private void Update()
        {
            transform.Translate(0, Speed * Time.deltaTime, 0);
            transform.localScale = (transform.localScale) + (Vector3.one * Time.deltaTime * ScaleFactor);
        }
    }
}