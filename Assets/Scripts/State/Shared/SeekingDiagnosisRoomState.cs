using Core;
using Hospital.Locations;
using State.Patient;
using UnityEngine;
using UnityEngine.AI;

namespace State.Shared
{
    public class SeekingDiagnosisRoomState : IState
    {
        private readonly NavMeshAgent _agent;
        private readonly CharacterType _type;

        private DiagnosisRoom _room;
        private readonly RotationHandler _rotationHandler;
        private Animator _animator;
        private static readonly int Walking = Animator.StringToHash("Walking");

        public SeekingDiagnosisRoomState(NavMeshAgent agent, CharacterType type, Animator animator)
        {
            _agent = agent;
            _type = type;
            _animator = animator;
            _rotationHandler = _agent.GetComponent<RotationHandler>();
        }


        public void OnEnter()
        {
            // TODO: find one correctly
            _animator.SetBool(Walking, true);
            _room = Object.FindObjectOfType<DiagnosisRoom>();
            if (_type == CharacterType.Patient)
            {
                _agent.SetDestination(_room.PatientPosition);
            }
            else
            {
                _agent.SetDestination(_room.DoctorPosition);
            }
        }

        public void OnExit()
        {
            // _rotationHandler.ClearTarget();
        }

        public void Tick(float deltaTime)
        {
            if (_agent.transform.position == _agent.destination)
            {
                _animator.SetBool(Walking, false);
                Vector3 target = _type == CharacterType.Staff
                    ? _room.PatientPosition
                    : _room.DoctorPosition;
                _rotationHandler.SetTarget(target);
            }
        }
    }
}