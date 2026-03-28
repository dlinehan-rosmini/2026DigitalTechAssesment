using System.Collections;
using UnityEngine;

public class LightFadeScript : MonoBehaviour
{
    public float amountPerSecond;
    private Light l;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        l = GetComponent<Light>();
        StartCoroutine(fade());
    }

    private IEnumerator fade()
    {
        while (true)
        {
            l.intensity -= amountPerSecond / 10;
            yield return new WaitForSeconds(0.1f);
            if (l.intensity >= 0)
                yield return null;
        }
    }
}
