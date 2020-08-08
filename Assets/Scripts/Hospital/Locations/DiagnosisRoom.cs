using Patients;
using Staff;
using State.Patient;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hospital.Locations
{
    public class DiagnosisRoom : TwoSpotLocation<Doctor, Patient>, IPositionProvider
    {
        [SerializeField] private Transform patientLocation;
        [SerializeField] private Transform doctorLocation;

        public bool DoctorIsNeeded => Patient != null && StaffMember == null && PatientPresent;

        private void Awake()
        {
            Assert.IsNotNull(patientLocation, "Patient location was not set!");
            Assert.IsNotNull(doctorLocation, "doctorLocation location was not set!");
        }

        protected override void DoOnTriggerEnter(Collider other)
        {
            if (Patient != null && StaffMember != null)
            {
                Debug.Log("Assigning Dr: " + StaffMember.name + " a patient of: " + Patient.name);
                StaffMember.CurrentPatient = Patient;
            }
        }

        public Vector3 GetPosition(CharacterType type)
        {
            if (type == CharacterType.Patient)
            {
                return patientLocation.position;
            }

            return doctorLocation.position;
        }
    }
}