using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItemBase : MonoBehaviour
{
    protected Character _character;
    public virtual void SendData(Character character)
    {
        _character = character;
    }
    public virtual void DestroyGameObject()
    {

    }
    public T GetCharacterAllFeature<T>() where T : Feature, new()
    {
        List<Feature> features = _character.GetAllFeatureEffect<T>();
        Feature result = null;
        if (features.Count == 1)
        {
            result = features[0];
        }
        else if (features.Count > 1)
        {
            result = features[0];
            for (var i = 1; i < features.Count - 1; i++)
            {
                result = result.SumFeature(result, features[i]);
            }
        }
        return (T)result;
    }
}
