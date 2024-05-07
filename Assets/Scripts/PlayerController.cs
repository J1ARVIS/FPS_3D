using UnityEngine;
using UnityEngine.InputSystem;
using FPS.Input;

namespace FPS.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _moveTarget;
        [SerializeField] private Transform _lookTarget;
        [Space]
        [Header("Look")]
        [SerializeField] private float _lookSpeed;
        [SerializeField] private float _minRotationBound;
        [SerializeField] private float _maxRotationBound;
        [Space]
        [Header("Move")]
        [SerializeField] private float _moveSpeed;
        [Space]
        [Header("Jump")]
        [SerializeField] private float _jumpPower;
        [SerializeField] private float _groundCheckDistance;

        private InputControlerMain _inputControler;

        private float _xRotation;
        private float _yRotation;

        private void Awake()
        {
            _inputControler = new InputControlerMain();
            Cursor.lockState = CursorLockMode.Locked;

            _inputControler.Player.Jump.performed += Jump;
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (IsGrounded())
            {
                _moveTarget.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            }
        }
        private bool IsGrounded()
        {
            var rayOrigin = _moveTarget.transform.position;

            Debug.DrawLine(rayOrigin, rayOrigin + Vector3.down * _groundCheckDistance, Color.red);
            return Physics.Raycast(rayOrigin, Vector3.down, _groundCheckDistance);
        }

        private void Update()
        {
            Look();
        }
        private void Look()
        {
            var lookDirection = _inputControler.Player.Look.ReadValue<Vector2>();
            var modifiedLookDirection = lookDirection * _lookSpeed * Time.deltaTime;

            _yRotation -= modifiedLookDirection.y;
            _yRotation = Mathf.Clamp(_yRotation, -_maxRotationBound, _minRotationBound);

            _xRotation += modifiedLookDirection.x;

            _lookTarget.transform.localRotation = Quaternion.Euler(_yRotation, _xRotation, 0f);
        }

        private void FixedUpdate()
        {
            if (IsGrounded()) Move();
        }
        private void Move()
        {
            var moveDirection = _inputControler.Player.Walk.ReadValue<Vector2>();

            var velocityByInput = new Vector3(moveDirection.x, 0f, moveDirection.y) * _moveSpeed * Time.fixedDeltaTime;
            var relativeVelocity = _lookTarget.transform.TransformDirection(velocityByInput);

            _moveTarget.velocity = new Vector3(relativeVelocity.x, _moveTarget.velocity.y, relativeVelocity.z);
        }
        private void OnEnable()
        {
            _inputControler.Enable();
        }
        private void OnDisable()
        {
            _inputControler.Disable();
        }
    }
}