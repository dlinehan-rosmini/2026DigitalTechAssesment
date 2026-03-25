using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header ("Main Variables")]
    public bool isEnemyBullet;
    public Rigidbody rb;
    RaycastHit Fhit;
    RaycastHit Bhit;


    [Header("Bullet Variables")]
    public bool explodes;
    public GameObject explosion;
    public float bulletDamage;
    public int bulletArmorLevel;
    public float rayDistance;
    public LayerMask rayMask;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfHitAnything();
    }
    void CheckIfHitAnything()
    {
        //Forward Raycast
        if (Physics.Raycast(transform.position, transform.forward, out Fhit, rayDistance, rayMask))
        {
            if (isEnemyBullet)
                try
                {
                    Fhit.collider.gameObject.GetComponent<PlayerGunManager>().ChangeHealth(-bulletDamage, bulletArmorLevel);
                }
                catch
                {

                }
            Destroy(gameObject);
        }
        //Backward Raycast
        if (Physics.Raycast(transform.position, -transform.forward, out Bhit, rayDistance, rayMask))
        {
            if (isEnemyBullet)
                try
                {
                    Fhit.collider.gameObject.GetComponent<PlayerGunManager>().ChangeHealth(-bulletDamage, bulletArmorLevel);
                }
                catch
                {

                }
            Destroy(gameObject);
        }
    }


    public void shootBullet(float speed, float damage, int armorLevel)
    {
        if (rb != null)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
            bulletDamage = damage;
            bulletArmorLevel = armorLevel;
        }
    }
}
