using Conditions;
using State.Patient;
using UnityEngine;
using UnityEngine.AI;

namespace State.Shared
{
    public class SeekingTreatmentRoomState : IState
    {
        private readonly NavMeshAgent _agent;
        private readonly Condition _condition;
        private readonly CharacterType _patient;

        public SeekingTreatmentRoomState(NavMeshAgent agent, Condition condition, CharacterType patient)
        {
            _agent = agent;
            _condition = condition;
            _patient = patient;
        }

        public void OnEnter()
        {
            Debug.Log("Seeking treatment for: " + _condition.conditionName);
        }

        public void OnExit()
        {
        }

        public void Tick(float deltaTime)
        {
        }
    }
}