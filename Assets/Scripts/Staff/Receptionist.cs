using System;
using Hospital.Locations;
using State;
using State.Patient;
using State.Shared;
using UnityEngine;
using UnityEngine.AI;

namespace Staff
{
    public class Receptionist : MonoBehaviour, IReceptionVisitor, IRoomSeeker
    {
        [SerializeField] private float lookSpeed = 200f;

        private StateMachine _stateMachine;
        private NavMeshAgent _agent;
        private Animator _animator;
        private ReceptionDesk _targetDesk;
        private bool _isCurrentlyManningStation;

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

        private StateMachine BuildStateMachine()
        {
            StateMachine sm = new StateMachine();
            IState idleState = new IdleState();

            IState seekingReceptionState =
                new SeekingReceptionState(_agent, this, CharacterType.Staff, _animator, this);
            sm.AddTransition(idleState, seekingReceptionState,
                () => !_isCurrentlyManningStation && ReceptionDeskAvailable());

            sm.SetState(idleState);

            return sm;
        }

        private bool ReceptionDeskAvailable()
        {
            // find the first desk that that has a free slot
            foreach (ReceptionDesk desk in FindObjectsOfType<ReceptionDesk>())
            {
                if (desk.HasRoomForStaff())
                {
                    _targetDesk = desk;
                    desk.RegisterStaff(this);
                    return true;
                }
            }

            return false;
        }


        #region IReceptionVisitor functions

        public void VisitReception()
        {
            _isCurrentlyManningStation = true;
        }

        public void LeaveReception()
        {
            _isCurrentlyManningStation = false;
        }

        public ReceptionDesk TargetReceptionDesk()
        {
            return _targetDesk;
        }

        #endregion

        public IPositionProvider GetPositionProvider()
        {
            return _targetDesk;
        }
    }
}