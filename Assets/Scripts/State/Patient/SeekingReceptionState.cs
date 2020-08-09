using Characters.Staff;
using Characters.Patients;
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
        private readonly ILocationSeeker<Receptionist, Characters.Patients.Patient> _locationSeeker;
        private readonly RotationHandler _rotationHandler;

        private TwoSpotLocation<Receptionist, Characters.Patients.Patient> _location;

        private static readonly int Walking = Animator.StringToHash("Walking");

        public SeekingReceptionState(NavMeshAgent agent, IReceptionVisitor visitor, CharacterType type,
            Animator animator, ILocationSeeker<Receptionist, Characters.Patients.Patient> locationSeeker)
        {
            _agent = agent;
            _visitor = visitor;
            _type = type;
            _animator = animator;
            _locationSeeker = locationSeeker;
            _rotationHandler = agent.GetComponent<RotationHandler>();
        }


        public void OnEnter()
        {
            _animator.SetBool(Walking, true);
            _location = _locationSeeker.GetLocation();
            _agent.SetDestination(_location.GetPosition(_type));
            _visitor.VisitReception();
        }

        public void OnExit()
        {
            _rotationHandler.ClearTarget();
            _visitor.LeaveReception();
        }

        public void Tick(float deltaTime)
        {
            if (_agent.transform.position == _agent.destination)
            {
                _animator.SetBool(Walking, false);
                _rotationHandler.SetTarget(_location.GetLookPosition(_type));
            }
        }
    }
}