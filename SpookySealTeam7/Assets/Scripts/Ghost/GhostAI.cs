using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(GhostMovement))]
    [RequireComponent(typeof(GhostVisibility))]
    public class GhostAI : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform modelTransform;
     
        private GhostMovement _ghostMovement;
        private GhostVisibility _ghostVisibility;

        private void Awake()
        {
            _ghostMovement = GetComponent<GhostMovement>();
            _ghostVisibility = GetComponent<GhostVisibility>();
        }

        private void Update()
        {
            Vector3 direction = playerTransform.position - modelTransform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation = Quaternion.Euler(-90, lookRotation.eulerAngles.y, lookRotation.eulerAngles.z);
            modelTransform.rotation = lookRotation;
            
            if (_ghostMovement.IsStandingStill())
            {
                _ghostVisibility.StandStill();
            }
            else
            {
                _ghostVisibility.ResetVisibility();
            }
        }

        public void ShineBlackLight()
        {
            _ghostVisibility.ResetVisibility();
            _ghostMovement.Move();
        }
    }
}