using Ghost;
using UnityEngine;

public class BlackLight : MonoBehaviour
{
    private bool _lightActive;
    private Light _lantern;
    
    public void SetLightActive(bool active) 
    {
        _lightActive = active;
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
            
            // Raycast from the light to see if it hits anything
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            int maxDist = 100;
            int layerMask = 1 << 7; // Only hit objects on the "Blacklight" layer (7)
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, fwd, out hitInfo, maxDist, layerMask))
            {
                hitInfo.collider.gameObject.GetComponent<GhostAI>().ShineBlackLight();
            }
        }
        else
        {
            _lantern.gameObject.SetActive(false);
        }
    }
}
