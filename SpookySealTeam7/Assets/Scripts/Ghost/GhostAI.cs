using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Renderer _ghostRenderer;
    private Vector3 _lastPosition;
    private bool _isFading;
    private float _standStillTimer;

    public int newLayer = 7; 
    public float fadeDelay = 1f;
    
    void Start()
    {
        _ghostRenderer = GetComponent<Renderer>();
        _lastPosition = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, _lastPosition) > 0.01f)
        {
            _standStillTimer = 0f;
            _isFading = false;
            _ghostRenderer.enabled = true;
        }
        else
        {
            _standStillTimer += Time.deltaTime;

            if (_standStillTimer >= fadeDelay && !_isFading)
            {
                StandStill();
            }
        }

        _lastPosition = transform.position;
    }

    void StandStill()
    {
        if (_ghostRenderer.enabled && !_isFading)
        {
            StartCoroutine(FadeOutAndChangeLayer());
        }
    }
    
    private IEnumerator FadeOutAndChangeLayer()
    {
        _isFading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDelay && _isFading)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.layer = newLayer;
        _ghostRenderer.enabled = false;
        // TODO: remove this renderer thing
    }
}
