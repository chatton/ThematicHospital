using System.Linq;
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

        public static T[] FindClosestObjectsOfType<T>(this GameObject go) where T : MonoBehaviour
        {
            return Object.FindObjectsOfType<T>()
                .OrderBy(d => Vector3.Distance(go.transform.position, d.transform.position)).ToArray();
        }

        public static T FindClosestObjectOfType<T>(this GameObject go) where T : MonoBehaviour
        {
            T[] all = FindClosestObjectsOfType<T>(go);
            if (all.Length == 0)
            {
                return null;
            }

            return all[0];
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