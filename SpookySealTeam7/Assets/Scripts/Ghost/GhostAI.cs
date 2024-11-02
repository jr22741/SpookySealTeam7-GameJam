using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    public class GhostAI : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private Vector3 _lastPosition;
        private bool _isFading;
        private bool _isVisible = true;
        private bool _isPanicking;
        private float _standStillTimer;

        [SerializeField] private float panicDuration = 1f;
        [SerializeField] private int gameLayer = 6;
        [SerializeField] private int blackLightLayer = 7; 
        [SerializeField] private float fadeDelay = 1f;
        [SerializeField] private float wanderRadius = 10f;
    
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _lastPosition = transform.position;
            StartCoroutine(Panic());
        }

        void Update()
        {
            if (_isPanicking)
            {
                Move();
            }
            
            if (Vector3.Distance(transform.position, _lastPosition) > 0.001f)
            {
                _standStillTimer = 0f;
                _isFading = false;
                _isVisible = true;
                gameObject.layer = gameLayer;
            }
            else
            {
                _standStillTimer += Time.deltaTime;

                if (_standStillTimer >= fadeDelay && !_isFading && !_isPanicking)
                {
                    StandStill();
                }
            }

            _lastPosition = transform.position;
        }

        void StandStill()
        {
            if (_isVisible && !_isFading)
            {
                StartCoroutine(FadeOutAndChangeLayer());
            }
        }

        void Move()
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius; 
            randomDirection.y = 0;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 1.0f, NavMesh.AllAreas))
            {
                _navMeshAgent.SetDestination(hit.position);
            }
        }

        IEnumerator Panic()
        {
            yield return new WaitForSeconds(1f);
            _isPanicking = true;
            yield return new WaitForSeconds(panicDuration);
            _isPanicking = false;
        }

        public void ShineBlackLight()
        {
            _isFading = false;
            _standStillTimer = 0f;
            _isVisible = true;

            StartCoroutine(Panic());
        }
    
        private IEnumerator FadeOutAndChangeLayer()
        {
            _isFading = true;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDelay && _isFading)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gameObject.layer = blackLightLayer;
            _isVisible = false;
        }
    }
}
