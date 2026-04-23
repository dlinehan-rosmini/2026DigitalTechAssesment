using UnityEngine;

public class BulletTrailScript : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * moveSpeed, ForceMode.Acceleration);
    }

}
