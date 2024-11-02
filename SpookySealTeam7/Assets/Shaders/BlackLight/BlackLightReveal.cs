using UnityEngine;
[ExecuteInEditMode]
public class BlackLightReveal : MonoBehaviour
{
    [SerializeField] Material Mat;
    [SerializeField] Light SpotLight;
	
	void Update ()
    {
        Mat.SetVector("_MyLightPosition",  SpotLight.transform.position);
        Mat.SetVector("_MyLightDirection", -SpotLight.transform.forward );
        Mat.SetFloat("_MyLightAngle", SpotLight.spotAngle);
    }//Update() end
}
