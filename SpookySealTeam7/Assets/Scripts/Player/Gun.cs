using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField] private float extendSpeed = 10.0f;
    [SerializeField] private float maxRayLength = 10.0f;
    [SerializeField] private float acceleration = 16.0f;
    [SerializeField] private ParticleSystem suck;
    [SerializeField] private ParticleSystem[] bolts;

    [SerializeField] private Image suckBar;
    [SerializeField] private float suckAmount;
    [SerializeField] private float maxSuck;

    [SerializeField] private int counter;

    private float _rayLength;
    private bool _gunActive;
    private float _attractionSpeed;

    public void SetGunActive(bool active)
    {
        if ((_gunActive && !active) || (!_gunActive && active)) {
            if (counter > 5) {
                _gunActive = active && (suckAmount > 0);
            } else {
                counter++;
            }
        } else if (_gunActive && suckAmount <= 0) {
            _gunActive = false;
        } else {
            _gunActive = active && (suckAmount > 0);
        }
    }

    public bool GetGunActive()
    {
        return _gunActive;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Turn emission off to begin with
        var emission = suck.emission;
        emission.enabled = false;
        foreach (var bolt in bolts)
        {
            emission = bolt.emission;
            emission.enabled = false;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //edit suckbar
        suckBar.fillAmount = suckAmount / maxSuck;


        ParticleSystem.EmissionModule emission;
        if (_gunActive)
        {

            //edit suck stamina
            suckAmount -= 25.0f *Time.deltaTime;
            if (suckAmount < 0) suckAmount = 0;

            if (suckAmount > 10) {

                // Enable emission on lightning when firing
                foreach (var bolt in bolts)
                {
                    emission = bolt.emission;
                    emission.enabled = true;
                    emission = bolt.emission;
                    emission.enabled = true;
                    // extend particle system over time
                    if (_rayLength < maxRayLength) _rayLength += extendSpeed * Time.deltaTime;
                    var shape = bolt.shape;
                    shape.length = _rayLength;
                }
            }
            
            // Raycast from the gun to see if it hits anything
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            int maxDist = 100;
            int layerMask = (1 << 6) + (1 << 7); // Only hit objects on the "Ghost" layer (6) and the "BlackLight" layer (7)
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, fwd, out hitInfo, maxDist, layerMask) && suckAmount > 10)
            {
                // Enable emission on suck when hitting ghost
                emission = suck.emission;
                emission.enabled = true;
                
                Vector3 ghostToGunVec = (transform.position - hitInfo.transform.position).normalized;
                _attractionSpeed += acceleration * Time.deltaTime;
                hitInfo.transform.position += ghostToGunVec * (_attractionSpeed * Time.deltaTime);
                
                // If the ghost reaches the gun, destroy it
                if (Vector3.Distance(hitInfo.transform.position, transform.position) < 0.6f)
                {
                    Destroy(hitInfo.collider.gameObject);
                    if (GameObject.FindGameObjectsWithTag("Ghost").Length == 0)
                    {
                        print("All ghosts destroyed!");
                    }
                }
            }
            else
            {
                emission = suck.emission;
                emission.enabled = false;
                _attractionSpeed = 0f;
            }
        } 
        else 
        {

            suckAmount += 20.0f * Time.deltaTime;
            if (suckAmount > maxSuck) suckAmount = maxSuck;

            _rayLength = 0;
            emission = suck.emission;
            emission.enabled = false;
            foreach (var bolt in bolts)
            {
                emission = bolt.emission;
                emission.enabled = false;
            }
            _attractionSpeed = 0f;
        }
    }
}
