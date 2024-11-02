using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Gun _gun;
    private Light _light;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gun = GetComponentInChildren<Gun>();
        _light = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1").Equals(1.0f)) {
            _gun.SetGunActive(true);
        } else {
            _gun.SetGunActive(false);
        }

        if (Input.GetAxis("Fire2").Equals(1.0f)) {
            _light.SetLightActive(true);
        } else {
            _light.SetLightActive(false);
        }
    }
}
