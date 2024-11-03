using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GhostUI : MonoBehaviour
    {
        [SerializeField] private GameObject ghost;
        [SerializeField] private Sprite aliveSprite;
        [SerializeField] private Sprite deadSprite;
        private SpriteRenderer _ghostImage;
        
        void Start()
        {
            _ghostImage = GetComponent<SpriteRenderer>();
            if (_ghostImage != null && aliveSprite != null && deadSprite != null)
            {
                _ghostImage.sprite = aliveSprite;
            }
        }

        void Update()
        {
            if (ghost == null)
            {
                _ghostImage.sprite = deadSprite;
            }
        }
    }
}
