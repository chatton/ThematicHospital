using UnityEngine;
using UnityEngine.AI;
using Util;

namespace Core
{
    public class RotationHandler : MonoBehaviour
    {
        [SerializeField] float lookSpeed = 500f;

        private NavMeshAgent _agent;
        private Vector3? _lookTarget;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (_lookTarget != null)
            {
                _agent.transform.SmoothLookAt(_lookTarget.Value, Time.deltaTime * lookSpeed);
            }
        }

        public void SetTarget(Vector3 target)
        {
            _lookTarget = target;
        }
        public void ClearTarget()
        {
            _lookTarget = null;
        }
    }
}