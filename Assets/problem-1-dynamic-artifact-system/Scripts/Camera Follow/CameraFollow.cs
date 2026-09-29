using UnityEngine;

namespace skillveri_Assignment
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform player;
        [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -6f);

        [Header("Smoothness")]
        [SerializeField] private float followSpeed = 8f;
        [SerializeField] private float lookSpeed = 10f;

        private void LateUpdate()
        {
            if (player == null) return;

            Vector3 desiredPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            Vector3 lookTarget = player.position + Vector3.up * 1.5f;
            Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
        }
    }
}
