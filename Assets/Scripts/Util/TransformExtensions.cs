using UnityEngine;

namespace Util
{
    public static class TransformExtensions
    {
        public static void SmoothLookAt(this Transform t, Vector3 target, float step)
        {
            Vector3 lTargetDir = target - t.position;
            lTargetDir.y = 0.0f;
            t.rotation = Quaternion.RotateTowards(t.rotation,
                Quaternion.LookRotation(lTargetDir),
                step);
        }
    }
}