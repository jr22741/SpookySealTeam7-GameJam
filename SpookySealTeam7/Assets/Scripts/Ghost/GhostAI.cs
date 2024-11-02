using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(GhostMovement))]
    [RequireComponent(typeof(GhostVisibility))]
    [RequireComponent(typeof(GhostPanic))]
    public class GhostAI : MonoBehaviour
    {
        private GhostMovement _ghostMovement;
        private GhostVisibility _ghostVisibility;
        private GhostPanic _ghostPanic;

        private void Awake()
        {
            _ghostMovement = GetComponent<GhostMovement>();
            _ghostVisibility = GetComponent<GhostVisibility>();
            _ghostPanic = GetComponent<GhostPanic>();
        }

        private void Update()
        {
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
