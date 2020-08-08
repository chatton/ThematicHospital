using Core;
using Hospital.Locations;
using State.Shared;
using UnityEngine;
using UnityEngine.AI;
using Util;

namespace State.Patient
{
    public enum CharacterType
    {
        Staff,
        Patient
    }

    public class SeekingReceptionState : IState
    {
        private readonly NavMeshAgent _agent;
        private readonly IReceptionVisitor _visitor;
        private readonly CharacterType _type;
        private readonly Animator _animator;
        private readonly float _lookSpeed;
        private ReceptionDesk _targetReceptionDesk;
        private readonly RotationHandler _rotationHandler;
        
        private static readonly int Walking = Animator.StringToHash("Walking");
        // private static readonly int Talking = Animator.StringToHash("Talking");

        public SeekingReceptionState(NavMeshAgent agent, IReceptionVisitor visitor, CharacterType type,
            Animator animator, float lookSpeed)
        {
            _agent = agent;
            _visitor = visitor;
            _type = type;
            _animator = animator;
            _lookSpeed = lookSpeed;
            _rotationHandler = agent.GetComponent<RotationHandler>();
        }

        // private static ReceptionDesk FindReception()
        // {
        //     // TODO: find closest reception
        //     // TODO: handle no reception existing
        //     return Object.FindObjectOfType<ReceptionDesk>();
        // }

        public void OnEnter()
        {
            _animator.SetBool(Walking, true);
            _targetReceptionDesk = _visitor.TargetReceptionDesk();
            if (_type == CharacterType.Patient)
            {
                _agent.SetDestination(_targetReceptionDesk.PatientPosition);
            }
            else
            {
                _agent.SetDestination(_targetReceptionDesk.ReceptionistPosition);
            }


            _visitor.VisitReception();
        }

        public void OnExit()
        {
            _rotationHandler.ClearTarget();
            _visitor.LeaveReception();
            // _animator.SetBool(Talking, false);
        }

        public void Tick(float deltaTime)
        {
            if (_agent.transform.position == _agent.destination)
            {
                _animator.SetBool(Walking, false);
                // _animator.SetBool(Talking, true);
                Vector3 target = _type == CharacterType.Staff
                    ? _targetReceptionDesk.PatientPosition
                    : _targetReceptionDesk.ReceptionistPosition;
                _rotationHandler.SetTarget(target);
            }
        }
    }
}