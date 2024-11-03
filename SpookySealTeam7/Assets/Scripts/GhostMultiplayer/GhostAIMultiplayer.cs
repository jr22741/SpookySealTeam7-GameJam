using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using System.Collections;

namespace Ghost
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(GhostMovementMultiplayer))]
    [RequireComponent(typeof(GhostVisibilityMultiplayer))]
    public class GhostAIMultiplayer : NetworkBehaviour
    {
        private Transform playerTransform;
        [SerializeField] private Transform modelTransform;
        private ClientNetworkTransform _transform;
     
        private GhostMovementMultiplayer _ghostMovement;
        private GhostVisibilityMultiplayer _ghostVisibility;

        private void Awake()
        {   
            _ghostMovement = GetComponent<GhostMovementMultiplayer>();
            _ghostVisibility = GetComponent<GhostVisibilityMultiplayer>();
            _transform = GetComponent<ClientNetworkTransform>();
        }

        private void Update()
        {
            playerTransform = GameObject.Find("MainCamera").transform;
            Vector3 direction = playerTransform.position - modelTransform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation = Quaternion.Euler(-90, lookRotation.eulerAngles.y, lookRotation.eulerAngles.z);
            modelTransform.rotation = lookRotation;
            
            if (_ghostMovement.IsStandingStill())
            {
                if (!IsServer) {
                    return;
                }

                _ghostVisibility.StandStill();
            }
            else
            {
                _ghostVisibility.ResetVisibility();
            }
        }

        private IEnumerator InitialScatter()
        {
            if (IsServer) {
                yield return new WaitForSeconds(1f);
                _ghostVisibility.ResetVisibility();
                _ghostMovement.Move();
            }

        }

        private IEnumerator PeriodicallyScatter()
        {   
            if (IsServer) {
                while (true)
                {
                    float waitTime = Random.Range(25f, 50f);
                    yield return new WaitForSeconds(waitTime);
                    _ghostVisibility.ResetVisibility();
                    _ghostMovement.Move();
                }
            }
        }

        public void ShineBlackLight()
        {
            if (!IsServer || !IsOwner) {
                ScareServerRpc();
            }

            _ghostVisibility.ResetVisibility();
            _ghostMovement.Move();
        }

        [ServerRpc(RequireOwnership = false)]
        private void ScareServerRpc() {
            ShineBlackLight();
        }

    }
}