using System.Collections;
using UnityEngine;

namespace Ghost
{
    public class GhostAI : MonoBehaviour
    {
        private Vector3 _lastPosition;
        private bool _isFading;
        private bool _isVisible = true;
        private bool _isPanicking;
        private float _standStillTimer;

        public float panicSpeed = 8f;
        public float panicDuration = 1f;
        public int gameLayer;
        public int blackLightLayer = 7; 
        public float fadeDelay = 1f;
    
        void Start()
        {
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
            //TODO: Implement the movement logic for the ghost when it is panicking
            Vector3 randomDirection = Random.insideUnitSphere; 
            randomDirection.y = 0; 
            randomDirection.Normalize();

            transform.position += randomDirection * (panicSpeed * Time.deltaTime);
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
