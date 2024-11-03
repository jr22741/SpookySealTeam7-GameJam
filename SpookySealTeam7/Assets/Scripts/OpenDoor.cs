using UnityEngine;
using Unity.Netcode;


public class OpenDoor : NetworkBehaviour {
    [SerializeField] private Vector3 translation;
    [SerializeField] private GameObject ghost;
    [SerializeField] private Vector3 spawn;

    public void OpenDoors() {
        OpenServerRpc();
        Debug.Log("Door Opening");
    }

    [ServerRpc]
    private void OpenServerRpc() {
        OpenClientRpc();
    }

    [ClientRpc]
    private void OpenClientRpc()
    {
        transform.Translate(translation);
        Instantiate(ghost, spawn, Quaternion.identity);
    }
}
