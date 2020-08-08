using UnityEngine;

namespace State.Shared
{
    public class IdleState : IState
    {
        public void Tick(float deltaTime)
        {
        }

        public void OnEnter()
        {
            Debug.Log("Entering Idle State");
        }

        public void OnExit()
        {
        }
    }
}