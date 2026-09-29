using UnityEngine;

namespace skillveri_Assignment
{
    public class CameraOrbit : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 200f;
        [SerializeField] private float minPitch = -90f;
        [SerializeField] private float maxPitch = 90f;
        [SerializeField] private Transform Playerbody;
        [SerializeField] private bool IsLookatOny;
        [Header("Camera Settings")]
        [SerializeField] private Vector3 offset = new Vector3(0f, 12f, -8f);
        [SerializeField] private float followSpeed = 5f;
        [SerializeField] private Vector3 cameraRotation = new Vector3(55f, 0f, 0f);
        private float Xrotation = 0;

        private void Start()
        {
            if (!IsLookatOny)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                if (Playerbody == null)
                    return;

                Vector3 targetPosition = Playerbody.position + offset;

                transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    followSpeed * Time.deltaTime
                );
            }
        }

        private void Update()
        {
            if (!IsLookatOny)
            {
                LikeaFpsMove();
            }
        }
        private void LateUpdate()
        {
            if (Playerbody == null)
                return;

            Vector3 targetPosition = Playerbody.position + offset;

            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                followSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Euler(cameraRotation);
        }
        void LikeaFpsMove()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            Xrotation -= mouseY;
            Xrotation = Mathf.Clamp(Xrotation, minPitch, maxPitch);
            transform.localRotation = Quaternion.Euler(Xrotation, 0, 0);
            Playerbody.Rotate(Vector3.up * mouseX);
        }
    }
}
