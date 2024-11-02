using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private bool gunActive;

    public void SetGunActive(bool active)
    {
        gunActive = active;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gunActive)
        {
            // Raycast from the gun to see if it hits anything
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            int maxDist = 100;
            int layerMask = 1 << 6; // Only hit objects on the "Ghost" layer (6)
            if (Physics.Raycast(transform.position, fwd, maxDist, layerMask))
            {
                print("Gun has hit object!");
            }
        }
    }
}
