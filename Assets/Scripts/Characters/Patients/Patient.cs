using System.Linq;
using Characters.Staff;
using Conditions;
using Hospital.Locations;
using State;
using State.Patient;
using State.Shared;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Patients
{
    public class Patient : MonoBehaviour, IReceptionVisitor, IRoomSeeker, ILocationSeeker<Receptionist, Patient>
    {
        [SerializeField] private float lookSpeed = 200f;
        [SerializeField] public Condition condition;

        private StateMachine _stateMachine;
        private NavMeshAgent _agent;
        private bool _hasGoneToReception;
        private Animator _animator;
        public bool IsCheckedIn { get; private set; }

        public bool HasBeenDiagnosed { get; set; }


        private Room _room;
        private IPositionProvider _positionProvider;

        private ReceptionDesk _targetDesk;

        // private DiagnosisRoom _targetRoom;
        private TreatmentRoom _targetTreatmentRoomRoom;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _stateMachine = BuildStateMachine();
        }


        private void Update()
        {
            _stateMachine.Tick(Time.deltaTime);
        }

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
                if (room.HasRoomForPatient())
                {
                    // _positionProvider = room;
                    _room = room;
                    room.RegisterPatient(this);
                    return true;
                }
            }

            return false;
        }

        private bool ReceptionDeskAvailable()
        {
            ReceptionDesk[] allDesks = FindObjectsOfType<ReceptionDesk>()
                .OrderBy(d => Vector3.Distance(transform.position, d.transform.position)).ToArray();

            foreach (ReceptionDesk desk in allDesks)
            {
                if (desk.HasRoomForPatient())
                {
                    _targetDesk = desk;
                    _positionProvider = desk;
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

            TreatmentRoom[] treatmentRooms = FindObjectsOfType<TreatmentRoom>();
            foreach (TreatmentRoom room in treatmentRooms)
            {
                if (room.CanTreat(condition) && room.HasRoomForPatient())
                {
                    room.RegisterPatient(this);
                    _positionProvider = room;
                    return true;
                }
            }

            return false;
        }

        private StateMachine BuildStateMachine()
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

            // if there is no current reception desk available, we can line up behind one
            // sm.AddTransition(idleState, lineUpState, () => !_hasGoneToReception && !ReceptionDeskAvailable() && _targetDesk.CanLineUp());

            // if we are lined up, we will go to reception at the next possible chance
            // sm.AddTransition(lineUpState, seekingReceptionState, () => _targetDesk.HasRoomForPatient());

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


        public IPositionProvider GetPositionProvider()
        {
            return _positionProvider;
            // if (IsCheckedIn)
            // {
            //     return _targetRoom;
            // }
            // else if (_targetDesk != null)
            // {
            //     return _targetDesk;
            // }
            //
            // throw new Exception(" There was no position provider!");
        }


        // TreatmentRoom<Doctor, Patient> IRoomSeeker<TreatmentRoom<Doctor, Patient>>.GetTargetRoom()
        // {
        //     return _targetTreatmentRoomRoom;
        // }
        public TwoSpotLocation<Receptionist, Patient> GetLocation()
        {
            return _targetDesk;
        }
    }
}