using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    public class GhostMovement : MonoBehaviour
    {
        [SerializeField] private float wanderRadius = 50f;
        private NavMeshAgent _navMeshAgent;
        private Vector3 _lastPosition;
        private float _standStillThreshold = 0.001f;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _lastPosition = transform.position;
        }

        public void Move()
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection.y = Random.Range(0, 2) == 0 ? 1 : 6;

            if (NavMesh.SamplePosition(randomDirection, out var hit, wanderRadius, NavMesh.AllAreas))
            {
                _navMeshAgent.SetDestination(hit.position);
            }
        }

        public bool IsStandingStill()
        {
            bool isStandingStill = Vector3.Distance(transform.position, _lastPosition) < _standStillThreshold;
            _lastPosition = transform.position;
            return isStandingStill;
        }
    }
}