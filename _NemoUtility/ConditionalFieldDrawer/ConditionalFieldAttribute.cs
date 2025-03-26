using UnityEngine;

namespace NemoUtility
{
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public string conditionField;

        public ConditionalFieldAttribute(string conditionField)
        {
            this.conditionField = conditionField;
        }
    }
}
