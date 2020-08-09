using System;
using Characters.Patients;
using Characters.Staff;
using Conditions;
using Staff;
using State.Patient;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hospital.Locations
{
    public abstract class TreatmentRoom : TwoSpotLocation<Doctor, Patient>, IPositionProvider
    {
        [SerializeField] protected Transform patientLocation;
        [SerializeField] protected Transform doctorLocation;
        public bool DoctorIsNeeded => Patient != null && StaffMember == null && PatientPresent;

        public abstract bool CanTreat(Condition condition);

        private void Awake()
        {
            Assert.IsNotNull(patientLocation, "patientLocation != null");
            Assert.IsNotNull(doctorLocation, "doctorLocation != null");
        }


        public Vector3 GetPosition(CharacterType type)
        {
            if (type == CharacterType.Patient)
            {
                return patientLocation.transform.position;
            }

            return doctorLocation.transform.position;
        }
    }
}