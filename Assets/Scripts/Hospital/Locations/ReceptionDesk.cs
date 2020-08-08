using System;
using Conditions;
using Patients;
using Staff;
using State.Patient;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hospital.Locations
{
    public class ReceptionDesk : TwoSpotLocation<Receptionist, Patient>, IPositionProvider
    {
        [SerializeField] private Transform patientLocation;
        [SerializeField] private Transform receptionLocation;
        [SerializeField] private float secondsForCheckIn = 5f;

        private float _elapsedCheckInTime;

        public Vector3 PatientPosition => patientLocation.position;
        public Vector3 ReceptionistPosition => receptionLocation.position;


        private void Awake()
        {
            Assert.IsNotNull(patientLocation, "Patient location was not set!");
            Assert.IsNotNull(receptionLocation, "Receptionist location was not set!");
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

            if (StaffMember != null)
            {
                _elapsedCheckInTime += Time.deltaTime;
            }

            if (StaffMember != null && _elapsedCheckInTime >= secondsForCheckIn)
            {
                _elapsedCheckInTime = 0;
                Debug.Log("Checking in patient: " + patient.name);
                patient.CheckIn();
            }
        }

        public Vector3 GetPosition(CharacterType type)
        {
            if (type == CharacterType.Patient)
            {
                return PatientPosition;
            }
            else
            {
                return ReceptionistPosition;
            }
        }
    }
}