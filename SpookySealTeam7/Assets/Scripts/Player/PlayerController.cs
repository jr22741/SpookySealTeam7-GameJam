using UnityEngine;

namespace player
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterController _controller;
        private GameObject _cam;
        private Gun _gun;
        private BlackLight _light;
        private float _verticalVelocity;
        private Vector3 _rotation;
        private Vector3 _camRotation;
        private bool _grounded;
        private float groundedTimer;
        private bool _paused;
        [SerializeField] private float playerSpeed = 5.0f;
        [SerializeField] private float mouseSpeed = 100.0f;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float gravityValue = -9.81f;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _controller = gameObject.AddComponent<CharacterController>();
            _cam = GameObject.FindGameObjectWithTag("MainCamera");
            _gun = gameObject.GetComponentInChildren<Gun>();
            _light = gameObject.GetComponentInChildren<BlackLight>();
            _rotation = transform.localEulerAngles;
            _camRotation = _cam.transform.localEulerAngles;
        }

        // Update is called once per frame
        void Update()
        {
            // Check for pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Invert cursor lock status and pause
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
                _paused = !_paused;
            }
            
            // Ignore movement if paused
            if (_paused) return;
            
            // grounded timer to not infinitely jump
            _grounded = _controller.isGrounded;
            if (_grounded)
            {
                groundedTimer = 0.2f;
            }
            if (groundedTimer > 0)
            {
                groundedTimer -= Time.deltaTime;
            }
            if (_grounded && _verticalVelocity < 0)
            {
                _verticalVelocity = 0f;
            }
            
            // gravity
            _verticalVelocity += gravityValue * Time.deltaTime;
            
            // input
            Vector3 move = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // player jump
            if (Input.GetButtonDown("Jump") && groundedTimer > 0)
            {
                groundedTimer = 0;
                _verticalVelocity += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            }
            
            move.y = _verticalVelocity;
            _controller.Move(move * (Time.deltaTime * playerSpeed));
            
            
            // player rotation
            float h = Input.GetAxis("Mouse X") * (Time.deltaTime * mouseSpeed);
            _rotation.y += h;
            transform.localEulerAngles = _rotation;
            
            // camera rotation
            float v = Input.GetAxis("Mouse Y") * (Time.deltaTime * mouseSpeed);
            _camRotation.x -= v;
            _camRotation.x = Mathf.Clamp(_camRotation.x, -90.0f, 90.0f);
            _cam.transform.localEulerAngles = _camRotation;
            
            // OTHER INPUTS
            if (!Input.GetAxis("Fire1").Equals(0.0f) && !_light.GetLightActive()) {
                _gun.SetGunActive(true);
            } else if (!Input.GetAxis("Fire2").Equals(0.0f) && !_gun.GetGunActive()) {
                _light.SetLightActive(true);
            } else {
                _light.SetLightActive(false);
                _gun.SetGunActive(false);
            }
        }
    }
}
