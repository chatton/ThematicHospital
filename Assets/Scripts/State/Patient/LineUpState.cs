using Core;
using Hospital.Locations;
using UnityEngine;
using UnityEngine.AI;

namespace State.Patient
{
    public class LineUpState : IState
    {
        private Characters.Patients.Patient _patient;
        private ReceptionDesk _desk;
        private NavMeshAgent _agent;
        private Animator _animator;
        private static readonly int Walking = Animator.StringToHash("Walking");
        private RotationHandler _rotator;

        public LineUpState(Characters.Patients.Patient patient)
        {
            _patient = patient;

            _agent = patient.GetComponent<NavMeshAgent>();
            _animator = patient.GetComponentInChildren<Animator>();
            _rotator = patient.GetComponent<RotationHandler>();
        }

        public void OnEnter()
        {
            _desk = _patient.TargetReceptionDesk();
            // _agent.SetDestination(_desk.GetNextLinePos());
            _animator.SetBool(Walking, true);
        }

        public void OnExit()
        {
        }

        public void Tick(float deltaTime)
        {
            // if (_agent.transform.position == _agent.destination)
            // {
            //     _rotator.SetTarget(_desk.ReceptionistPosition);
            //     _animator.SetBool(Walking, false);
            // }
        }
    }
}