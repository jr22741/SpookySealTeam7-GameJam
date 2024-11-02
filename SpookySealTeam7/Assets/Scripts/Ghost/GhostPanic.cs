using System.Collections;
using UnityEngine;

namespace Ghost
{
    public class GhostPanic : MonoBehaviour
    {
        [SerializeField] private float panicDuration = 5f;
        public bool IsPanicking { get; private set; }
        
        public void TriggerPanic()
        {
            if (!IsPanicking)
            {
                StartCoroutine(PanicCoroutine());
            }
        }
        
        private IEnumerator PanicCoroutine()
        {
            IsPanicking = true;
            yield return new WaitForSeconds(panicDuration);
            IsPanicking = false;
        }
    }
}