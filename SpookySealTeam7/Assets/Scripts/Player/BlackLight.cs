using Ghost;
using UnityEngine;

public class BlackLight : MonoBehaviour
{
    [SerializeField] private float lightRadius = 5.0f;
    private bool _lightActive;
    private Light _light;
    
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
        _light = GetComponentInChildren<Light>();
        _light.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_lightActive)
        {
            // enable light and raise lantern
            _light.gameObject.SetActive(true);
            Vector3 pos = transform.localPosition;
            pos.y = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 0.5f);
            
            // Check spherical area around the light to see if it hits anything
            int layerMask = 1 << 7; // Only hit objects on the "Blacklight" layer (7)
            Collider[] ghosts = Physics.OverlapSphere(transform.position, lightRadius, layerMask);
            foreach(Collider ghost in ghosts)
            {
                // if nothing is in the way
                if (!Physics.Linecast(transform.position, ghost.transform.position, (1 << 16)))
                {
                    ghost.gameObject.GetComponent<GhostAI>().ShineBlackLight();
                }
            }
        }
        else
        {
            _light.gameObject.SetActive(false);
            Vector3 pos = transform.localPosition;
            pos.y = -0.75f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 0.5f);
        }
    }
}
