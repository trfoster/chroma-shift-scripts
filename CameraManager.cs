using UnityEngine;

namespace ChromaShift {
    public class CameraManager : MonoBehaviour {
        [SerializeField] private int levelHeight = 40;
        
        private void Start() {
            UpdateCameraSize();
        }

        private void OnValidate() {
            UpdateCameraSize();
        }

        private void UpdateCameraSize() {
            int screenHeight = Screen.height;
            Camera cameraComponent = gameObject.GetComponent<Camera>();
            Camera childCamera = transform.GetChild(0).GetComponent<Camera>();

            float pixelsPerUnit = screenHeight / (float)levelHeight;

            float cameraSize = screenHeight / pixelsPerUnit * 0.5f;
            cameraComponent.orthographicSize = cameraSize;
            childCamera.orthographicSize = cameraSize;
        }
    }
}
