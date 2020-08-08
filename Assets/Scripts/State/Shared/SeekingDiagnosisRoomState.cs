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

        private IPositionProvider _room;
        private readonly RotationHandler _rotationHandler;
        private Animator _animator;
        private readonly IRoomSeeker _seeker;
        private static readonly int Walking = Animator.StringToHash("Walking");

        public SeekingDiagnosisRoomState(NavMeshAgent agent, CharacterType type, Animator animator, IRoomSeeker seeker)
        {
            _agent = agent;
            _type = type;
            _animator = animator;
            _seeker = seeker;

            _rotationHandler = _agent.GetComponent<RotationHandler>();
        }


        public void OnEnter()
        {
            _room = _seeker.GetPositionProvider();
            _animator.SetBool(Walking, true);
            _agent.SetDestination(_room.GetPosition(_type));
        }

        public void OnExit()
        {
            _rotationHandler.ClearTarget();
        }

        public void Tick(float deltaTime)
        {
            if (_agent.transform.position == _agent.destination)
            {
                _animator.SetBool(Walking, false);
                Vector3 target = _room.GetPosition(_type);
                _rotationHandler.SetTarget(target);
            }
        }
    }
}