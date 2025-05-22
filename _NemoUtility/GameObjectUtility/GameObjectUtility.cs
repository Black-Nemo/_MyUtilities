using UnityEngine;

namespace NemoUtility
{
    public static class GameObjectUtility
    {
        public static void SetLayerAllChildren(GameObject obj, int layer)
        {
            if (obj == null) { return; }
            obj.layer = layer;

            foreach (Transform child in obj.transform)
            {
                SetLayerAllChildren(child.gameObject, layer);
            }
        }

        public static void SetLayerAllChildren(GameObject obj, LayerMask layer)
        {
            int layerIndex = Mathf.RoundToInt(Mathf.Log(layer.value, 2));
            SetLayerAllChildren(obj, layerIndex);
        }
    }
}
