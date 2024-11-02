using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Ghost
{
    public class GhostAI : MonoBehaviour
    {
        private const int GameLayer = 6;
        private const int BlackLightLayer = 7;
        
        private NavMeshAgent _navMeshAgent;
        private Vector3 _lastPosition;
        private bool _isFading;
        private bool _isVisible = true;
        private bool _isPanicking;
        private float _standStillTimer;

        [SerializeField] private float panicDuration = 5f;
        [SerializeField] private float fadeDelay = 1f;
        [SerializeField] private float wanderRadius = 10f;
    
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _lastPosition = transform.position;
            gameObject.layer = GameLayer;
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
                gameObject.layer = GameLayer;
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

            if (NavMesh.SamplePosition(randomDirection, out var hit, 10.0f, NavMesh.AllAreas))
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

            gameObject.layer = BlackLightLayer;
            _isVisible = false;
        }
    }
}
