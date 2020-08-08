using Patients;
using Staff;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hospital.Locations
{
    public class DiagnosisRoom : TwoSpotLocation<Doctor, Patient>
    {
        [SerializeField] private Transform patientLocation;
        [SerializeField] private Transform doctorLocation;

        public bool DoctorIsNeeded => Patient != null && StaffMember == null && PatientPresent;

        private void Awake()
        {
            Assert.IsNotNull(patientLocation, "Patient location was not set!");
            Assert.IsNotNull(doctorLocation, "doctorLocation location was not set!");
        }

        public Vector3 PatientPosition => patientLocation.position;
        public Vector3 DoctorPosition => doctorLocation.position;


        protected override void DoOnTriggerEnter(Collider other)
        {
            if (Patient != null && StaffMember != null)
            {
                Debug.Log("Assigning Dr: " + StaffMember.name + " a patient of: " + Patient.name);
                StaffMember.CurrentPatient = Patient;
            }
        }
    }
}