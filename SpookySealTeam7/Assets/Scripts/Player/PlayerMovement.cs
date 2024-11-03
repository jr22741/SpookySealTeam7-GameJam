using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private Vector3 _rotation;
    private bool _groundedPlayer;
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float mouseSpeed = 100.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private ClientNetworkTransform _transform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        _controller = gameObject.AddComponent<CharacterController>();
        _rotation = transform.localEulerAngles;
        _transform = GetComponent<ClientNetworkTransform>();
    }

    // Update is called once per frame
    void Update()
    {   

        if (!IsOwner)
        {
            return;
        }

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


        _playerVelocity.y += gravityValue * Time.deltaTime;
        
        MoveServerRpc(_rotation, _playerVelocity);

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
