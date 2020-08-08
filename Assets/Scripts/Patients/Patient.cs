using Conditions;
using Hospital.Locations;
using State;
using State.Patient;
using State.Shared;
using UnityEngine;
using UnityEngine.AI;

namespace Patients
{
    public class Patient : MonoBehaviour, IReceptionVisitor
    {
        [SerializeField] private float lookSpeed = 200f;
        [SerializeField] public Condition condition;

        private StateMachine _stateMachine;
        private NavMeshAgent _agent;
        private bool _hasGoneToReception;
        private Animator _animator;
        public bool IsCheckedIn { get; private set; }

        public bool HasBeenDiagnosed { get; set; }


        private ReceptionDesk _targetDesk;

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
            // TODO: find one that is available, not just one that exists
            DiagnosisRoom room = FindObjectOfType<DiagnosisRoom>();
            return room != null;
        }

        private bool ReceptionDeskAvailable()
        {
            foreach (ReceptionDesk desk in FindObjectsOfType<ReceptionDesk>())
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


        private StateMachine BuildStateMachine()
        {
            StateMachine sm = new StateMachine();

            IState idleState = new IdleState();
            IState seekingReceptionState =
                new SeekingReceptionState(_agent, this, CharacterType.Patient, _animator, lookSpeed);

            IState seekingDiagnosisState = new SeekingDiagnosisRoomState(_agent, CharacterType.Patient, _animator);
            IState seekingTreatmentState = new SeekingTreatmentRoomState(_agent, condition, CharacterType.Patient);

            // we want to go to the reception if we have not yet gone.
            sm.AddTransition(idleState, seekingReceptionState, () => !_hasGoneToReception && ReceptionDeskAvailable());

            // if the patient has been to reception, then we can go back to the idle state
            sm.AddTransition(seekingReceptionState, idleState, () => IsCheckedIn);

            // once the patient has been checked in, they need to seek a diagnosis room
            sm.AddTransition(idleState, seekingDiagnosisState, () => IsCheckedIn && DiagnosisRoomIsAvailable());

            // once the patient is diagnosed, they will then seek treatment
            sm.AddTransition(seekingDiagnosisState, seekingTreatmentState, () => HasBeenDiagnosed);

            // all patients come into the world in the idle state.
            sm.SetState(idleState);
            return sm;
        }
    }
}