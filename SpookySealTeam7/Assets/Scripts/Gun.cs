using UnityEngine;

public class Gun : MonoBehaviour
{
    private bool _gunActive;
    private float _attractionSpeed = 0f;

    public void SetGunActive(bool active)
    {
        _gunActive = active;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_gunActive)
        {
            // Raycast from the gun to see if it hits anything
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            int maxDist = 100;
            int layerMask = (1 << 6) + (1 << 7); // Only hit objects on the "Ghost" layer (6) and the "BlackLight" layer (7)
            RaycastHit hitInfo;
            print("casting ray");
            if (Physics.Raycast(transform.position, fwd, out hitInfo, maxDist, layerMask))
            {
                print("Gun has hit object!");
                Vector3 ghostToGunVec = (transform.position - hitInfo.transform.position).normalized;
                _attractionSpeed += 8f * Time.deltaTime;
                hitInfo.transform.position += ghostToGunVec * (_attractionSpeed * Time.deltaTime);
            } 
            else
            {
                _attractionSpeed = 0f;
            }
        } 
        else 
        {
            _attractionSpeed = 0f;
        }
    }
}
