using Core;
using Hospital.Locations;
using Staff;
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
        private ReceptionDesk _targetReceptionDesk;
        private readonly RotationHandler _rotationHandler;
        private readonly IRoomSeeker _seeker;

        private static readonly int Walking = Animator.StringToHash("Walking");
        // private static readonly int Talking = Animator.StringToHash("Talking");

        public SeekingReceptionState(NavMeshAgent agent, IReceptionVisitor visitor, CharacterType type,
            Animator animator, IRoomSeeker seeker)
        {
            _agent = agent;
            _visitor = visitor;
            _type = type;
            _animator = animator;
            _rotationHandler = agent.GetComponent<RotationHandler>();
            _seeker = seeker;
        }


        public void OnEnter()
        {
            _animator.SetBool(Walking, true);
            _targetReceptionDesk = _visitor.TargetReceptionDesk();

            IPositionProvider provider = _seeker.GetPositionProvider();
            _agent.SetDestination(provider.GetPosition(_type));
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