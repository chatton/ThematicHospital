using UnityEngine;
using UnityEngine.Assertions;

namespace CameraControls
{
    public class CameraMovement : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            Assert.IsNotNull(_camera, "Main camera was null!");
        }

        private void Update()
        {
            // TODO: clamp movement
            float xAxisValue = Input.GetAxis("Horizontal");
            float zAxisValue = Input.GetAxis("Vertical");
            _camera.transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue), Space.World);
        }
    }
}