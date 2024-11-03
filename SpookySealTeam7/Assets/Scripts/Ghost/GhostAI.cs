using System.Collections;
using System.Collections.Generic;
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

        private void Start()
        {
            StartCoroutine(InitialScatter());
            StartCoroutine(PeriodicallyScatter());
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

        private IEnumerator InitialScatter()
        {
            yield return new WaitForSeconds(1f);
            _ghostVisibility.ResetVisibility();
            _ghostMovement.Move();
        }

        private IEnumerator PeriodicallyScatter()
        {
            while (true)
            {
                float waitTime = Random.Range(25f, 50f);
                yield return new WaitForSeconds(waitTime);
                _ghostVisibility.ResetVisibility();
                _ghostMovement.Move();
            }
        }

        public void ShineBlackLight()
        {
            _ghostVisibility.ResetVisibility();
            _ghostMovement.Move();
        }
    }
}