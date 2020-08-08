using System;
using Patients;
using Staff;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hospital.Locations
{
    public class ReceptionDesk : MonoBehaviour
    {
        [SerializeField] private Transform patientLocation;
        [SerializeField] private Transform receptionLocation;
        [SerializeField] private float secondsForCheckIn = 5f;

        private float _elapsedCheckInTime;
        private Patient _patient;
        private Receptionist _receptionist;

        public Vector3 PatientPosition => patientLocation.position;
        public Vector3 ReceptionistPosition => receptionLocation.position;


        public void RegisterPatient(Patient patient)
        {
            _patient = patient;
        }


        public void RegisterReceptionist(Receptionist receptionist)
        {
            _receptionist = receptionist;
        }

        public bool IsFreeForPatient()
        {
            return _patient == null;
        }

        public bool IsFreeForReceptionist()
        {
            return _receptionist == null;
        }


        private void Awake()
        {
            Assert.IsNotNull(patientLocation, "Patient location was not set!");
            Assert.IsNotNull(receptionLocation, "Receptionist location was not set!");
        }


        private void OnTriggerEnter(Collider other)
        {
            Receptionist receptionist = other.GetComponent<Receptionist>();
            if (receptionist == null)
            {
                return;
            }

            if (_receptionist == null)
            {
                _receptionist = receptionist;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // If a receptionist is exiting, they are no longer manning the station
            Receptionist receptionist = other.GetComponent<Receptionist>();
            if (receptionist != null)
            {
                if (receptionist == _receptionist)
                {
                    _receptionist = null;
                }
            }

            // if a patient exits, we clear up the slot for a new patient
            Patient patient = other.GetComponent<Patient>();
            if (patient != null)
            {
                if (patient == _patient)
                {
                    _patient = null;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Patient patient = other.GetComponent<Patient>();
            if (patient == null)
            {
                // whatever entered the zone wasn't a patient so we can ignore it
                return;
            }

            if (patient.IsCheckedIn)
            {
                return;
            }

            if (_receptionist != null)
            {
                _elapsedCheckInTime += Time.deltaTime;
            }

            if (_receptionist != null && _elapsedCheckInTime >= secondsForCheckIn)
            {
                _elapsedCheckInTime = 0;
                Debug.Log("Checking in patient: " + patient.name);
                patient.CheckIn();
            }
        }
    }
}