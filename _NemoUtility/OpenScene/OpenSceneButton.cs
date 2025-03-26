using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace NemoUtility
{
    [RequireComponent(typeof(Button))]
    public class OpenSceneButton : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(sceneName); });
        }
    }
}