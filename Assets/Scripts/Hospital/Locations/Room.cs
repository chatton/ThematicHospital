using Characters.Patients;
using Characters.Staff;
using Staff;
using State.Patient;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hospital.Locations
{
    public class Room : TwoSpotLocation<Doctor, Patient>
    {
        // [SerializeField] private Transform patientLocation;
        // [SerializeField] private Transform doctorLocation;

        // the condition that this room can treat
        [SerializeField] private ConditionType treatableCondition;

        private Machine _machine;
        public bool DoctorIsNeeded => Patient != null && StaffMember == null && PatientPresent;

        private void Awake()
        {
            Assert.IsNotNull(patientLocation, "Patient location was not set!");
            Assert.IsNotNull(staffLocation, "doctorLocation location was not set!");
        }

        protected override void DoOnTriggerEnter(Collider other)
        {
            if (Patient != null && StaffMember != null)
            {
                Debug.Log("Assigning Dr: " + StaffMember.name + " a patient of: " + Patient.name);
                StaffMember.CurrentPatient = Patient;
            }
        }

        // public Vector3 GetPosition(CharacterType type)
        // {
        //     if (type == CharacterType.Patient)
        //     {
        //         return patientLocation.position;
        //     }
        //
        //     return doctorLocation.position;
        // }

        public Machine GetMachine()
        {
            if (_machine == null)
            {
                _machine = GetComponentInChildren<Machine>();
            }

            Assert.IsNotNull(_machine, "Machine was null!");
            return _machine;
        }
    }
}