using UnityEngine;
using UnityEngine.UI;

namespace skillveri_Assignment
{
    public class PlayerChecker : MonoBehaviour
    {
        public PlayerMovement playerMovement;
        public EnemyAI enemyAI;
        public float teleportDistance = 2f;
        public Button DynamicExitBTN;
        public GameObject DynamicInpuGameObect;
        public Transform Enemyvisiblittyarea;
        void Start()
        {
            DynamicExitBTN?.onClick.AddListener(makePlayerReset);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 direction =
            (playerMovement.transform.position - enemyAI.transform.position).normalized;

                direction.y = 0f;

                Vector3 targetPosition =
                    enemyAI.transform.position + direction * teleportDistance;

                playerMovement.Teleport(targetPosition);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                playerMovement.SetPlayerMovementStop(false);
                DynamicInpuGameObect.SetActive(true);
                DynamicExitBTN.gameObject.SetActive(true);
            }
        }
        // void OnTriggerEnter(Collider other)
        // {
        //     print(other.gameObject.name);
        //     if (other.gameObject.tag == "Finish")
        //     {
        //         // playerMovement.transform.position =
        //         // new Vector3(transform.position.x - 4, playerMovement.transform.position.y, transform.position.z - 1);
        //         playerMovement.SetPlayerMovementStop(false);
        //         DynamicInpuGameObect.SetActive(true);
        //         DynamicExitBTN.gameObject.SetActive(true);
        //     }
        // }
        void makePlayerReset()
        {
            playerMovement.SetPlayerMovementStop(true);
            DynamicInpuGameObect.SetActive(false);
            DynamicExitBTN.gameObject.SetActive(false);
        }
    }
}
