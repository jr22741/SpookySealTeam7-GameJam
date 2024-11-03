using System.Collections;
using UnityEngine;

namespace Ghost
{
    public class GhostVisibility : MonoBehaviour
    {
        private const int GameLayer = 6;
        private const int BlackLightLayer = 7;

        private bool _isFading;
        private bool _isVisible = true;

        [SerializeField] private GameObject model;
        [SerializeField] private float fadeDelay = 1f;

        private void Start()
        {
            gameObject.layer = GameLayer;
            model.layer = GameLayer;
        }

        public void StandStill()
        {
            if (_isVisible && !_isFading)
            {
                StartCoroutine(FadeOutAndChangeLayer());
            }
        }

        public void ResetVisibility()
        {
            _isFading = false;
            _isVisible = true;
            gameObject.layer = GameLayer;
            model.layer = GameLayer;
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
            model.layer = BlackLightLayer;
            _isVisible = false;
        }
    }
}