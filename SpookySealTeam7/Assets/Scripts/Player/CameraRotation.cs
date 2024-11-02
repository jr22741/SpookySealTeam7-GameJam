using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 100.0f;
    private Vector3 _rotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Mouse Y") * (Time.deltaTime * mouseSpeed);
        _rotation.x -= v;
        _rotation.x = Mathf.Clamp(_rotation.x, -90.0f, 90.0f);
        transform.localEulerAngles = _rotation;
    }
}
