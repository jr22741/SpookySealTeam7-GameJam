using Ghost;
using UnityEngine;

public class BlackLight : MonoBehaviour
{
    [SerializeField] private float lightRadius = 5.0f;
    private bool _lightActive;
    private Light _lantern;
    
    public void SetLightActive(bool active) 
    {
        _lightActive = active;
    }

    public bool GetLightActive()
    {
        return _lightActive;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lantern = GetComponentInChildren<Light>();
        _lantern.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_lightActive)
        {
            // enable lantern
            _lantern.gameObject.SetActive(true);
            
            // Check spherical area around the light to see if it hits anything
            int layerMask = 1 << 7; // Only hit objects on the "Blacklight" layer (7)
            Collider[] ghosts = Physics.OverlapSphere(transform.position, lightRadius, layerMask);
            foreach(Collider ghost in ghosts)
            {
                // if nothing is in the way
                if (!Physics.Linecast(transform.position, ghost.transform.position, (1 << 16)))
                {
                    if (ghost.gameObject.GetComponent<GhostAI>()) {
                        ghost.gameObject.GetComponent<GhostAI>().ShineBlackLight();
                    } else if (ghost.gameObject.GetComponent<GhostAIMultiplayer>()) {
                        ghost.gameObject.GetComponent<GhostAIMultiplayer>().ShineBlackLight();
                    }
                }
            }
        }
        else
        {
            _lantern.gameObject.SetActive(false);
        }
    }
}
