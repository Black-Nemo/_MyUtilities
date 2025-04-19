using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NemoUtility
{
    public class MouseInfoManager : MonoBehaviour
    {
        [SerializeField] private GameObject InfoTextPrefab;
        [SerializeField] private Vector2 LocationText;
        [SerializeField] private Vector2 LocationTextMobile;

        [SerializeField] private GameObject ImagePrefab;
        [SerializeField] private Vector2 LocationImage;

        [SerializeField] private Transform canvas;

        GameObject rememberTextObject;
        GameObject rememberImageObject;

        public static MouseInfoManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Active(string str, Color bgColor)
        {
            if (rememberTextObject != null) { Destroy(rememberTextObject); }
            rememberTextObject = Instantiate(InfoTextPrefab, canvas);
            rememberTextObject.transform.SetParent(canvas);
            rememberTextObject.GetComponent<MouseInfo>().Text.text = str;
            rememberTextObject.GetComponent<Image>().color = bgColor;
        }
        public void DeActive()
        {
            Destroy(rememberTextObject);
        }

        public GameObject ActiveImage(Sprite sprite)
        {
            if (rememberImageObject != null) { Destroy(rememberImageObject); }
            rememberImageObject = Instantiate(ImagePrefab, canvas);
            rememberImageObject.transform.SetParent(canvas);
            rememberImageObject.GetComponent<Image>().sprite = sprite;
            rememberImageObject.transform.position += new Vector3(LocationText.x, LocationText.y, 0);
            return rememberImageObject;
        }
        public void DeActiveImage()
        {
            Destroy(rememberImageObject);
        }

        private void Update()
        {
            if (rememberTextObject != null)
            {
                Vector2 locationText;
                if (Application.isMobilePlatform)
                {
                    locationText = LocationTextMobile;
                }
                else
                {
                    locationText = LocationText;
                }

                var rectTrnsfrm = rememberTextObject.GetComponent<RectTransform>();
                Vector2 screenSize = new Vector2(Screen.width, Screen.height);
                Vector2 mousePos = Input.mousePosition;

                rememberTextObject.transform.position = mousePos;
                if ((rectTrnsfrm.sizeDelta.x * canvas.localScale.x) + mousePos.x + locationText.x > screenSize.x)
                {
                    rememberTextObject.transform.position = new Vector3(screenSize.x - (rectTrnsfrm.sizeDelta.x * canvas.localScale.x), mousePos.y + locationText.y, 0);
                }
                else
                {
                    rememberTextObject.transform.position += new Vector3(locationText.x, locationText.y, 0);
                }
            }
            if (rememberImageObject != null)
            {
                rememberImageObject.transform.position = Input.mousePosition + new Vector3(LocationImage.x, LocationImage.y, 0);
            }
        }
    }
}