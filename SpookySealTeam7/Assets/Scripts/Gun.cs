using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float extendSpeed = 1.0f;
    [SerializeField] private float maxRayLength = 5.0f;
    [SerializeField] private ParticleSystem suck;
    [SerializeField] private ParticleSystem[] spirals;
    private float _rayLength;
    private bool _gunActive;

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
        ParticleSystem.EmissionModule emission;
        if (_gunActive)
        {
            // enable emission on spirals
            foreach (var spiral in spirals)
            {
                emission = spiral.emission;
                emission.enabled = true;
                emission = spiral.emission;
                emission.enabled = true;
                // extend particle system over time
                if (_rayLength < maxRayLength) _rayLength += extendSpeed * Time.deltaTime;
                var shape = spiral.shape;
                shape.position = new Vector3(shape.position.x, shape.position.y, _rayLength);
                shape.length = _rayLength;
            }
            
            // Raycast from the gun to see if it hits anything
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            int maxDist = 100;
            int layerMask = (1 << 6) + (1 << 7); // Only hit objects on the "Ghost" layer (6)
            emission = suck.emission;
            if (Physics.Raycast(transform.position, fwd, maxDist, layerMask))
            {
                print("Gun has hit object!");
                emission.enabled = true;
            }
            else
            {
                emission.enabled = false;
            }
        }
        else
        {
            _rayLength = 0;
            emission = suck.emission;
            emission.enabled = false;
            foreach (var spiral in spirals)
            {
                emission = spiral.emission;
                emission.enabled = false;
            }
        }
    }
}
