using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _cameraTransfrom;
    [SerializeField] private float _verticalTurnSensitivity = 0.5f;
    [SerializeField] private float _horizontalTurnSensitivity = 0.2f;
    [SerializeField] private float _verticalMinAngle = -89f;
    [SerializeField] private float _verticalMaxAngle = 89f;

    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private Vector2 _moveDirection;
    private Vector2 _lookDirection;
    private float _cameraAngle = 0f;
    private Transform _transform;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _cameraAngle = _cameraTransfrom.localEulerAngles.x;
        _transform = transform;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        _moveDirection = _playerInput.Player.Move.ReadValue<Vector2>();
        _lookDirection = _playerInput.Player.Look.ReadValue<Vector2>();

        Move();
        Look();
    }

    private void Move()
    {
        if (_moveDirection.sqrMagnitude <= 0.1f)
            return;

        Vector3 forward = Vector3.ProjectOnPlane(_cameraTransfrom.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(_cameraTransfrom.right, Vector3.up).normalized;

        Vector3 offset = forward * _moveDirection.y + right * _moveDirection.x;
        float scaleMoveSpeed = _moveSpeed * Time.deltaTime;
        offset *= scaleMoveSpeed;

        if (_characterController.isGrounded)
        {
            _characterController.Move(offset + Physics.gravity * Time.deltaTime);
        }
        else
        {
            _characterController.Move(Physics.gravity * Time.deltaTime);
        }
    }

    private void Look()
    {
        if (_lookDirection.sqrMagnitude <= 0.1f)
            return;

        _cameraAngle -= _lookDirection.y * _verticalTurnSensitivity;
        _cameraAngle = Mathf.Clamp(_cameraAngle, _verticalMinAngle, _verticalMaxAngle);
        _cameraTransfrom.localEulerAngles = Vector3.right * _cameraAngle;
        _transform.Rotate(Vector3.up * _horizontalTurnSensitivity * _lookDirection.x);
    }
}
