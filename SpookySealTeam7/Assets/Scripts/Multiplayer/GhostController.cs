using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    public float speed = 20;

    private ClientNetworkTransform _transform;

    private void Start()
    {
        _transform = GetComponent<ClientNetworkTransform>();
    }
    private void Update()
    {
        if (!IsOwner) return;
        var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _transform.transform.position += movement * speed * Time.deltaTime;
    }

}
