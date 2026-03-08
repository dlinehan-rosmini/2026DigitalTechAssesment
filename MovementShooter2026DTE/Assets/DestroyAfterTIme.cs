using UnityEngine;

public class DestroyAfterTIme : MonoBehaviour
{
    public float time;

    private void Start()
    {
        Destroy(gameObject, time);
    }
}
