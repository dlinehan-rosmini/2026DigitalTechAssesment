using System.Collections;
using UnityEngine;

public class LightFadeScript : MonoBehaviour
{
    public float time;
    private Light light;
    private float startingIntensity;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light = GetComponent<Light>();
        startingIntensity = light.intensity;
        StartCoroutine(fade());
    }

    private IEnumerator fade()
    {
        while (true)
        {
            light.intensity -= (startingIntensity / time);
            yield return new WaitForSeconds(startingIntensity/time);
            if (light.intensity > 0 ) 
                yield return null;
        }
    }
}
