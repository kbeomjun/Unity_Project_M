using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTr;

    private float _moveSpeed = 5.0f;

    private PlayerControls _controls;
    private CharacterController _characterController;

    private void Awake()
    {
        _controls = new PlayerControls();
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveInput = _controls.Player.Move.ReadValue<Vector2>();
        
        Vector3 forward = _cameraTr.forward;
        Vector3 right = _cameraTr.right;

        forward.y = 0.0f;
        right.y = 0.0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
        _characterController.Move(moveDirection * _moveSpeed * Time.deltaTime);
    }

}
