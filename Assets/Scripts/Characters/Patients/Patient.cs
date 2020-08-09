using System.Linq;
using Characters.Staff;
using Conditions;
using Hospital.Locations;
using State;
using State.Patient;
using State.Shared;
using UnityEngine;
using UnityEngine.AI;
using static Util.Extensions;

namespace Characters.Patients
{
    public class Patient : Character, IReceptionVisitor, IRoomSeeker, ILocationSeeker<Receptionist, Patient>
    {
        [SerializeField] public Condition condition;

        private bool _hasGoneToReception;

        public bool IsCheckedIn { get; private set; }

        public bool HasBeenDiagnosed { get; set; }

        private ReceptionDesk _targetDesk;
        private TreatmentRoom _targetTreatmentRoomRoom;


        #region IReceptionVisitor functions

        public void VisitReception()
        {
            _hasGoneToReception = true;
        }

        public void LeaveReception()
        {
            IsCheckedIn = true;
        }

        public ReceptionDesk TargetReceptionDesk()
        {
            return _targetDesk;
        }

        # endregion

        public void CheckIn()
        {
            IsCheckedIn = true;
        }


        private bool DiagnosisRoomIsAvailable()
        {
            foreach (Room room in FindObjectsOfType<Room>())
            {
                if (room.HasRoomForPatient() && room.Type == RoomType.Diagnosis)
                {
                    _room = room;
                    room.RegisterPatient(this);
                    return true;
                }
            }

            return false;
        }

        private bool ReceptionDeskAvailable()
        {
            ReceptionDesk[] allDesks = gameObject.FindClosestObjectsOfType<ReceptionDesk>();
            foreach (ReceptionDesk desk in allDesks)
            {
                if (desk.HasRoomForPatient())
                {
                    _targetDesk = desk;
                    desk.RegisterPatient(this);
                    return true;
                }
            }

            return false;
        }


        private bool TreatmentRoomAvailable()
        {
            if (!HasBeenDiagnosed)
            {
                return false;
            }

            Room[] treatmentRooms = FindObjectsOfType<Room>().Where(r => r.Type == RoomType.Treatment).ToArray();
            foreach (Room room in treatmentRooms)
            {
                if (room.CanTreat(condition) && room.HasRoomForPatient())
                {
                    _room = room;
                    room.RegisterPatient(this);
                    return true;
                }
            }

            return false;
        }

        protected override StateMachine BuildStateMachine()
        {
            StateMachine sm = new StateMachine();

            IState idleState = new IdleState();
            IState seekingReceptionState =
                new SeekingReceptionState(_agent, this, CharacterType.Patient, _animator, this);

            // IState lineUpState = new LineUpState(this);

            IState seekingDiagnosisState =
                new SeekingDiagnosisRoomState(_agent, CharacterType.Patient, _animator, this);
            IState seekingTreatmentState =
                new SeekingTreatmentRoomState(_agent, condition, CharacterType.Patient, this, _animator);

            // we want to go to the reception if we have not yet gone.
            sm.AddTransition(idleState, seekingReceptionState, () => !_hasGoneToReception && ReceptionDeskAvailable());
            
            // if the patient has been to reception, then we can go back to the idle state
            sm.AddTransition(seekingReceptionState, idleState, () => IsCheckedIn);

            // once the patient has been checked in, they need to seek a diagnosis room
            sm.AddTransition(idleState, seekingDiagnosisState, () => IsCheckedIn && DiagnosisRoomIsAvailable());

            // once the patient is diagnosed, they will then seek treatment
            sm.AddTransition(seekingDiagnosisState, seekingTreatmentState,
                () => HasBeenDiagnosed && TreatmentRoomAvailable());

            // all patients come into the world in the idle state.
            sm.SetState(idleState);
            return sm;
        }

        public Room GetRoom()
        {
            return _room;
        }

        public TwoSpotLocation<Receptionist, Patient> GetLocation()
        {
            return _targetDesk;
        }
    }
}