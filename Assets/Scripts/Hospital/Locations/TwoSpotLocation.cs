using UnityEngine;

namespace Hospital.Locations
{
    public class TwoSpotLocation<TStaff, TPatient> : MonoBehaviour
    {
        protected TStaff StaffMember;

        protected TPatient Patient;

        public void RegisterStaff(TStaff staff)
        {
            StaffMember = staff;
        }

        public void RegisterPatient(TPatient patient)
        {
            Patient = patient;
        }

        public bool HasRoomForStaff()
        {
            return StaffMember == null;
        }

        public bool HasRoomForPatient()
        {
            return Patient == null;
        }

        private void OnTriggerExit(Collider other)
        {
            // If a receptionist is exiting, they are no longer manning the station
            TStaff staff = other.GetComponent<TStaff>();
            if (staff != null)
            {
                if (staff.Equals(StaffMember))
                {
                    StaffMember = default;
                }
            }

            // if a patient exits, we clear up the slot for a new patient
            TPatient patient = other.GetComponent<TPatient>();
            if (patient != null)
            {
                if (patient.Equals(Patient))
                {
                    Patient = default;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            TStaff staff = other.GetComponent<TStaff>();
            if (staff != null)
            {
                if (StaffMember == null)
                {
                    StaffMember = staff;
                }
            }

            TPatient patient = other.GetComponent<TPatient>();
            if (patient != null)
            {
                if (Patient == null)
                {
                    Patient = patient;
                }
            }
        }
    }
}