using Patients;
using Staff;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hospital.Locations
{
    public class DiagnosisRoom : MonoBehaviour
    {
        [SerializeField] private Transform patientLocation;
        [SerializeField] private Transform doctorLocation;


        private Doctor _doctor;
        private Patient _patient;

        public bool DoctorIsNeeded => _patient != null && _doctor == null;

        private void Awake()
        {
            Assert.IsNotNull(patientLocation, "Patient location was not set!");
            Assert.IsNotNull(doctorLocation, "doctorLocation location was not set!");
        }

        public Vector3 PatientPosition => patientLocation.position;
        public Vector3 DoctorPosition => doctorLocation.position;

        private void OnTriggerEnter(Collider other)
        {
            Patient patient = other.GetComponent<Patient>();
            if (patient != null)
            {
                _patient = patient;
            }


            Doctor doctor = other.GetComponent<Doctor>();
            if (doctor != null)
            {
                _doctor = doctor;
            }

     
            if (_patient != null && _doctor != null)
            {
                Debug.Log("Assigning Dr: " + _doctor.name + " a patient of: " + _patient.name);
                _doctor.CurrentPatient = _patient;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Patient patient = other.GetComponent<Patient>();
            if (patient != null)
            {
                _patient = null;
            }

            Doctor doctor = other.GetComponent<Doctor>();
            if (doctor != null)
            {
                _doctor = null;
            }
           
        }
    }
}