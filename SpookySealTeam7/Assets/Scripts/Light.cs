using UnityEngine;

public class Light : MonoBehaviour
{
    private bool _lightActive;
    
    public void SetLightActive(bool active) 
    {
        _lightActive = active;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_lightActive)
        {
            // Raycast from the light to see if it hits anything
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            int maxDist = 100;
            int layerMask = 1 << 7; // Only hit objects on the "Blacklight" layer (6)
            if (Physics.Raycast(transform.position, fwd, maxDist, layerMask))
            {
                print("Blacklight has hit object!");
            }
        }
    }
}
