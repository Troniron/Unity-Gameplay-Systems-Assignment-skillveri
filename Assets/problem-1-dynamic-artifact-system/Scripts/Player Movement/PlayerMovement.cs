using UnityEngine;
using UnityEngine.InputSystem;

namespace skillveri_Assignment
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float JumpHeight = 5f;
        [SerializeField] private float gravity = -20f;
        private const float GroundedStickVelocity = -2f;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] Transform groundCheck;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private Transform CameraObject;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 15f;
        private CharacterController controller;
        private Vector3 velocity;
        private Vector3 currentMoveVelocity;
        private Vector3 smoothVelocity;
        private bool _mIsgrounded;
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (InputChangerSingleton.Instance.ChangePlayerInput())
            {
                ApplyMovement_InputAction();
            }
            else
                ApplyMovement();
        }
        private void OnEnable()
        {
            if (moveAction != null)
                moveAction.action.Enable();
            if (jumpAction != null)
                jumpAction.action.Enable();
        }

        private void OnDisable()
        {
            if (moveAction != null)
                moveAction.action.Disable();
            if (jumpAction != null)
                jumpAction.action.Disable();
        }
        private void ApplyMovement()
        {

            _mIsgrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (_mIsgrounded && velocity.y < 0f)
                velocity.y = GroundedStickVelocity;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 inputDirection = Vector3.ClampMagnitude(
                transform.right * horizontal + transform.forward * vertical, 1f);

            Vector3 horizontalMove = inputDirection * moveSpeed;

            if (_mIsgrounded && Input.GetKeyDown(KeyCode.Space))
                velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);

            velocity.y += gravity * Time.deltaTime;

            Vector3 finalVelocity = horizontalMove;
            finalVelocity.y = velocity.y;

            controller.Move(finalVelocity * Time.deltaTime);
        }
        private void ApplyMovement_InputAction()
        {
            _mIsgrounded = Physics.CheckSphere(
                groundCheck.position,
                groundDistance,
                groundMask
            );

            if (_mIsgrounded && velocity.y < 0f)
                velocity.y = GroundedStickVelocity;

            Vector2 input = moveAction.action.ReadValue<Vector2>();

            float horizontal = input.x;
            float vertical = input.y;

            Vector3 inputDirection =
                transform.right * horizontal +
                transform.forward * vertical;

            inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);

            Vector3 horizontalMove = inputDirection * moveSpeed;

            if (_mIsgrounded && jumpAction.action.WasPressedThisFrame())
            {
                velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            Vector3 finalVelocity = horizontalMove;
            finalVelocity.y = velocity.y;

            controller.Move(finalVelocity * Time.deltaTime);
        }
        public void Teleport(Vector3 position)
        {
            controller.enabled = false;

            transform.position = position;

            velocity = Vector3.zero;
            currentMoveVelocity = Vector3.zero;
            smoothVelocity = Vector3.zero;

            controller.enabled = true;
        }
        public void SetPlayerMovementStop(bool Stateof)
        {
            transform.gameObject.SetActive(Stateof);
        }
    }
}
