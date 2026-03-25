using UnityEngine;

public class ThrowableScript : MonoBehaviour
{


    [Header("Variables")]
    public bool actingModel;
    public GameObject model;
    public bool StickOnContactPoint;
    public bool rotate = false;

    public bool explode;
    public float damageOnContact;
    public int armorLevel;

    private Vector3 startPos;

    public Rigidbody rb;
    public Collider collision;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        startPos = transform.position;
        if (actingModel)
        {
            rb.useGravity = false;
            collision.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyManager>() != null)
        {
            collision.gameObject.GetComponent<EnemyManager>().ChangeHealth(-damageOnContact, armorLevel);
        }
        float r = Random.Range(-100, 1);
        if (r <= 0 && StickOnContactPoint)
        {
            var thing = Instantiate(model, collision.GetContact(0).point + new Vector3(0,0,-1), Quaternion.LookRotation(-collision.GetContact(0).normal));
            thing.transform.parent = collision.gameObject.transform;
            thing.GetComponent<ThrowableScript>().actingModel = true;

            Destroy(gameObject);
        }
    }
    public void Throw(float f, Transform pos)
    {
        rb.AddForce(pos.forward * f, ForceMode.Impulse);
        if (rotate)
            rb.AddTorque(transform.right * f );


    }
}
