using Hospital;
using UnityEngine;

namespace Conditions
{
    [CreateAssetMenu(fileName = "Condition", menuName = "Conditions/Create")]
    public class Condition : ScriptableObject
    {
        public string conditionName;
        public ConditionType conditionType;
        public float baseSecondToDiagnosis;
        public float dieAfterSeconds;
        public bool IsFatal() => dieAfterSeconds <= 0f;
    }
}