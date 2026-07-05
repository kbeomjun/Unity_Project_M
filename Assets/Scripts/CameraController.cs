using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _playerTr;
    [SerializeField] private float _sensitivity = 0.2f;
    
    private float distance = 5.0f;
    private float height = 2.0f;
    private float _yaw;
    private float _pitch;

    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void LateUpdate()
    {
        Vector2 look = _controls.Player.Look.ReadValue<Vector2>();

        _yaw += look.x * _sensitivity;
        _pitch -= look.y * _sensitivity;
        _pitch = Mathf.Clamp(_pitch, -80.0f, 80.0f);

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0.0f);

        transform.position = _playerTr.position + rotation * new Vector3(0.0f, height, -distance);
        transform.rotation = rotation;
        _playerTr.rotation = Quaternion.Euler(0.0f, _yaw, 0.0f);
    }

}
