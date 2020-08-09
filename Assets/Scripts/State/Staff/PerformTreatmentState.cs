using Hospital;
using Hospital.Locations;
using State.Patient;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Util;

namespace State.Staff
{
    internal class PerformTreatmentState : IState
    {
        private readonly NavMeshAgent _agent;
        private readonly IRoomSeeker _seeker;
        private readonly Animator _animator;
        private readonly IMachineProvider _machineProvider;

        private IPositionProvider _positionProvider;
        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int On = Animator.StringToHash("On");

        public PerformTreatmentState(NavMeshAgent agent, IRoomSeeker seeker, Animator animator,
            IMachineProvider machineProvider)
        {
            _agent = agent;
            _seeker = seeker;
            _animator = animator;
            _machineProvider = machineProvider;
        }

        public void OnEnter()
        {
            _animator.SetBool(Walking, true);
            _positionProvider = _seeker.GetPositionProvider();
            _agent.SetDestination(_positionProvider.GetPosition(CharacterType.Staff));
        }

        public void OnExit()
        {
        }

        public void Tick(float deltaTime)
        {
            if (_agent.HasReachedDestination())
            {
                _animator.SetBool(Walking, false);
                _agent.transform.SmoothLookAt(_agent.destination, Time.deltaTime * 500f);
                Machine machine = _machineProvider.GetMachine();
                Assert.IsNotNull(machine, "The machine was null!");
                machine.GetComponent<Animator>().SetBool(On, true);
            }
        }
    }
}