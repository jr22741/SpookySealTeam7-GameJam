using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GhostUI : MonoBehaviour
    {
        [SerializeField] private GameObject ghost;
        [SerializeField] private RawImage ghostImage;
        [SerializeField] private Texture aliveSprite;
        [SerializeField] private Texture deadSprite;
        
        void Start()
        {
            if (ghostImage != null && aliveSprite != null && deadSprite != null)
            {
                ghostImage.texture = aliveSprite;
            }
            else
            {
                ghostImage.enabled = false;
            }
        }

        void Update()
        {
            if (ghost == null)
            {
                ghostImage.texture = deadSprite;
            }
        }
    }
}
