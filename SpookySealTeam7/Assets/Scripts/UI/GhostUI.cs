using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GhostUI : MonoBehaviour
    {
        [SerializeField] private GameObject ghost;
        [SerializeField] private Texture aliveSprite;
        [SerializeField] private Texture deadSprite;
        private RawImage _ghostImage;
        
        void Start()
        {
            _ghostImage = GetComponent<RawImage>();
            if (_ghostImage != null && aliveSprite != null && deadSprite != null)
            {
                _ghostImage.texture = aliveSprite;
            }
        }

        void Update()
        {
            if (ghost == null)
            {
                _ghostImage.texture = deadSprite;
            }
        }
    }
}
