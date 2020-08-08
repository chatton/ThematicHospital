using Conditions;
using Hospital.Locations;
using Staff;
using State.Patient;
using UnityEngine;
using UnityEngine.AI;

namespace State.Shared
{
    public class SeekingTreatmentRoomState : IState
    {
        private readonly NavMeshAgent _agent;
        private readonly Condition _condition;
        private readonly CharacterType _type;

        // private readonly IRoomSeeker<TreatmentRoom<Doctor, Patients.Patient>> _treatmentRoomSeeker;
        private readonly IRoomSeeker _treatmentRoomSeeker;

        private readonly Animator _animator;
        private static readonly int Walking = Animator.StringToHash("Walking");

        public SeekingTreatmentRoomState(NavMeshAgent agent, Condition condition, CharacterType type,
            IRoomSeeker treatmentRoomSeeker, Animator animator)
        {
            _agent = agent;
            _condition = condition;
            _type = type;
            _treatmentRoomSeeker = treatmentRoomSeeker;
            _animator = animator;
        }

        public void OnEnter()
        {
            Debug.Log("Seeking treatment for: " + _condition.conditionName);
            _agent.SetDestination(_treatmentRoomSeeker.GetPositionProvider().GetPosition(_type));
            _animator.SetBool(Walking, true);
        }

        public void OnExit()
        {
        }

        public void Tick(float deltaTime)
        {
        }
    }
}