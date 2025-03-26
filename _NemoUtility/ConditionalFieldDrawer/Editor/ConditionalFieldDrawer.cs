using UnityEditor;
using UnityEngine;

namespace NemoUtility
{
    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditional.conditionField);

            if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
            {
                if (conditionProperty.boolValue)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
            else
            {
                Debug.LogWarning("ConditionalField can only be used with boolean fields.");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditional.conditionField);

            if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
            {
                if (conditionProperty.boolValue)
                {
                    return EditorGUI.GetPropertyHeight(property, label);
                }
            }

            return 0;
        }
    }
}