using UnityEngine;

namespace skillveri_Assignment
{
    using UnityEngine;

    public class InputChangerSingleton : MonoBehaviour
    {
        public static InputChangerSingleton Instance { get; private set; }
        public bool ChangePlayerInputToNew;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        public bool ChangePlayerInput()
        {
            return ChangePlayerInputToNew;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
