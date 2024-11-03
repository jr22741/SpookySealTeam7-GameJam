using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

namespace Player
{
public class PlayerControllerMultiplayer : NetworkBehaviour {
        private CharacterController _controller;
        [SerializeField] private GameObject _cam;
        private GunMultiplayer _gun;
        private BlackLight _light;
        private float _verticalVelocity;
        private Vector3 _rotation;
        private Vector3 _camRotation;
        private GameObject _menus;
        private bool _grounded;
        private bool _paused;
        private float groundedTimer;
        [SerializeField] private float playerSpeed = 5.0f;
        [SerializeField] private float mouseSpeed = 100.0f;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float gravityValue = -9.81f;

        private ClientNetworkTransform _transform;
        
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _controller = gameObject.AddComponent<CharacterController>();
            _gun = gameObject.GetComponentInChildren<GunMultiplayer>();
            _light = gameObject.GetComponentInChildren<BlackLight>();
            _rotation = transform.localEulerAngles;
            _camRotation = _cam.transform.localEulerAngles;
            _transform = GetComponent<ClientNetworkTransform>();
            _menus = GameObject.Find("Menus");

            if (!IsOwner)
            {
                return;
            }

            _menus.GetComponent<CanvasGroup>().alpha = 0;
            _menus.GetComponent<CanvasGroup>().interactable = false;

            GameObject camera = GameObject.Find("/MainCamera");
            Debug.Log(camera);
            camera.transform.SetParent(_cam.transform);
            camera.transform.position = _cam.transform.position;
            camera.transform.rotation = _cam.transform.rotation;
            
        }

        public override void OnNetworkSpawn()
        {
            gameObject.transform.position = GameObject.Find("SpawnPoint").transform.position;
            // Server-side monster init script here
            base.OnNetworkSpawn();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            // Check for pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Invert cursor lock status and pause
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
                _menus.GetComponent<CanvasGroup>().alpha = (_menus.GetComponent<CanvasGroup>().alpha == 0) ? 1 : 0;
                _menus.GetComponent<CanvasGroup>().interactable = !_menus.GetComponent<CanvasGroup>().interactable;

                _paused = !_paused;
            }
            
            // Ignore movement if paused
            if (_paused) return;
            
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

        [ServerRpc]
        private void MoveServerRpc(Vector3 _rotation, Vector3 _playerVelocity) {
            MoveClientRpc(_rotation, _playerVelocity);
        }

        [ClientRpc]
        private void MoveClientRpc(Vector3 _rotation, Vector3 _playerVelocity)
        {
            transform.localEulerAngles = _rotation;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }
    }
}
