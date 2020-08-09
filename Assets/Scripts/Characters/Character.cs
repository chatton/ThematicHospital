using Hospital.Locations;
using State;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    public abstract class Character : MonoBehaviour
    {
        private StateMachine _stateMachine;
        private NavMeshAgent _agent;
        private bool _hasGoneToReception;
        private Animator _animator;
        private Room _room;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _stateMachine = BuildStateMachine();
            DoAwake();
        }

        protected virtual void DoAwake()
        {
        }

        private void Update()
        {
            _stateMachine.Tick(Time.deltaTime);
        }

        protected abstract StateMachine BuildStateMachine();
    }
}