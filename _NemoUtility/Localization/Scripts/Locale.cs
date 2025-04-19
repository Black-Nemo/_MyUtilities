using UnityEngine;

namespace NemoUtility
{
    [CreateAssetMenu(fileName = "new  Locale", menuName = "Locale")]
    public class Locale : ScriptableObject
    {
        public string Name;
        public string Code;
    }
}