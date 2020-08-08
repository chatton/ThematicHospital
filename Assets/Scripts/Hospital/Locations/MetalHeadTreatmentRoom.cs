using Conditions;
using Patients;
using Staff;
using State.Patient;
using UnityEngine;

namespace Hospital.Locations
{
    public class MetalHeadTreatmentRoom : TreatmentRoom
    {
        public override bool CanTreat(Condition condition)
        {
            return condition.conditionName == "MetalHead";
        }
    }
}