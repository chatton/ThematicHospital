using System;
using State;
using State.Patient;
using State.Shared;
using UnityEngine;
using UnityEngine.AI;

namespace Staff
{
    public class Receptionist : MonoBehaviour, IReceptionVisitor
    {
        [SerializeField] private float lookSpeed = 200f;

        private StateMachine _stateMachine;
        private NavMeshAgent _agent;
        private Animator _animator;

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
                new SeekingReceptionState(_agent, this, CharacterType.Staff, _animator, lookSpeed);
            sm.AddTransition(idleState, seekingReceptionState, () => !_isCurrentlyManningStation);
            sm.SetState(idleState);

            return sm;
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

        #endregion
    }
}