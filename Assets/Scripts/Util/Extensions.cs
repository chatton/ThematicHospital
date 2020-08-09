using UnityEngine;
using UnityEngine.AI;

namespace Util
{
    public static class Extensions
    {
        public static void SmoothLookAt(this Transform t, Vector3 target, float step)
        {
            Vector3 lTargetDir = target - t.position;
            lTargetDir.y = 0.0f;
            t.rotation = Quaternion.RotateTowards(t.rotation,
                Quaternion.LookRotation(lTargetDir),
                step);
        }

        public static bool HasReachedDestination(this NavMeshAgent agent)
        {
            if (agent.destination == agent.transform.position)
            {
                return true;
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || Mathf.Abs(agent.velocity.sqrMagnitude) < float.Epsilon)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}