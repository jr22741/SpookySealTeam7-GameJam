using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Gun _gun;
    private BlackLight _light;
    private Vector3 _playerVelocity;
    private Vector3 _rotation;
    private bool _groundedPlayer;
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float mouseSpeed = 100.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _controller = gameObject.AddComponent<CharacterController>();
        _gun = gameObject.GetComponentInChildren<Gun>();
        _light = gameObject.GetComponentInChildren<BlackLight>();
        _rotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }
        
        Vector3 move = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _controller.Move(move * (Time.deltaTime * playerSpeed));

        // player jump
        if (Input.GetButtonDown("Jump") && _groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }
        
        // player rotation
        float h = Input.GetAxis("Mouse X") * (Time.deltaTime * mouseSpeed);
        _rotation.y += + h;
        transform.localEulerAngles = _rotation;

        _playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
        
        // head rotation
        float v = Input.GetAxis("Mouse Y") * (Time.deltaTime * mouseSpeed);
        _rotation.x -= v;
        _rotation.x = Mathf.Clamp(_rotation.x, -90.0f, 90.0f);
        transform.localEulerAngles = _rotation;
        
        // OTHER INPUTS
        if (Input.GetAxis("Fire1").Equals(1.0f)) {
            _gun.SetGunActive(true);
        } else {
            _gun.SetGunActive(false);
        }

        if (Input.GetAxis("Fire2").Equals(1.0f)) {
            _light.SetLightActive(true);
        } else {
            _light.SetLightActive(false);
        }
    }
}
