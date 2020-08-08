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

        public Vector3 PatientPosition => patientLocation.position;
        public Vector3 ReceptionistPosition => receptionLocation.position;

        // _isManned should be true if there is a staff member at this reception table.
        private bool _isManned;

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

            _isManned = true;
        }

        private void OnTriggerExit(Collider other)
        {
            Receptionist receptionist = other.GetComponent<Receptionist>();
            if (receptionist == null)
            {
                return;
            }

            _isManned = false;
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

            if (_isManned)
            {
                _elapsedCheckInTime += Time.deltaTime;
            }

            if (_isManned && _elapsedCheckInTime >= secondsForCheckIn)
            {
                _elapsedCheckInTime = 0;
                Debug.Log("Checking in patient: " + patient.name);
                patient.CheckIn();
            }
        }
    }
}