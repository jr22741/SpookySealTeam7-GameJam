using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

namespace Ghost
{
    public class GhostMovementMultiplayer : NetworkBehaviour
    {
        [SerializeField] private float wanderRadius = 50f;
        private NavMeshAgent _navMeshAgent;
        private Vector3 _lastPosition;
        private float _standStillThreshold = 0.001f;
        private ClientNetworkTransform _transform;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _lastPosition = transform.position;
            _transform = GetComponent<ClientNetworkTransform>();
        }

        public void Move()
        {
            if (!IsServer) {
                return;
            }

            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection.y = 6;

            if (NavMesh.SamplePosition(randomDirection, out var hit, wanderRadius, NavMesh.AllAreas))
            {
                print(hit.position.y);
                MoveServerRpc(hit.position);
            }
        }

        public bool IsStandingStill()
        {
            bool isStandingStill = Vector3.Distance(transform.position, _lastPosition) < _standStillThreshold;
            _lastPosition = transform.position;
            return isStandingStill;
        }

        [ServerRpc]
        private void MoveServerRpc(Vector3 position) {
            MoveClientRpc(position);
        }

        [ClientRpc]
        private void MoveClientRpc(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
        }
    }
}