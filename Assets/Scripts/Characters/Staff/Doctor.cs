using Characters.Patients;
using Hospital;
using Hospital.Locations;
using Staff;
using State;
using State.Patient;
using State.Shared;
using State.Staff;
using UnityEngine.Assertions;

namespace Characters.Staff
{
    public class Doctor : BaseStaff, IRoomSeeker
    {
        private bool _isCurrentlyManningStation;
        public Patient CurrentPatient { get; set; }

        private Machine _machine;

        private bool ThereIsARoomWhichRequiredADoctor(RoomType roomType)
        {
            Room room = FindAvailableRoomWithDoctorNeededOfType(roomType);
            if (room == null)
            {
                return false;
            }

            _room = room;
            room.RegisterStaff(this);
            return true;
        }

        protected override StateMachine BuildStateMachine()
        {
            StateMachine sm = new StateMachine();
            IState idleState = new IdleState();
            IState seekingDiagnostics = new SeekingDiagnosisRoomState(_agent, CharacterType.Staff, _animator, this);
            IState performDiagnosis = new PerformDiagnosisState(this);
            IState preformTreatmentState = new PerformTreatmentState(_agent, this, _animator);


            sm.AddTransition(idleState, preformTreatmentState,
                () => ThereIsARoomWhichRequiredADoctor(RoomType.Treatment));

            // If there are any diagnostics rooms that need a doctor, we will head there.
            sm.AddTransition(idleState, seekingDiagnostics,
                () => ThereIsARoomWhichRequiredADoctor(RoomType.Diagnosis));

            // We can begin the diagnosis process as soon as we have a patient
            sm.AddTransition(seekingDiagnostics, performDiagnosis, () => CurrentPatient != null);

            // Once we have diagnosed the patient, we can go back to being idle
            sm.AddTransition(performDiagnosis, idleState, () => CurrentPatient == null);


            sm.SetState(idleState);
            return sm;
        }
    }
}