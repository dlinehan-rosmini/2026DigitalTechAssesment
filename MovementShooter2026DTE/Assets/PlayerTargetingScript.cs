using UnityEngine;

public class PlayerTargetingScript : MonoBehaviour
{
    [Header("Detection Settings")]
    public string targetTag = "Player";
    public float detectionRange = 20f;
    public float fieldOfViewAngle = 60f;
    public LayerMask obstructionMask;

    [Header("Rotation Settings")]
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 2f;

    [Header("Shootin")]
    public GameObject bullet;
    public float bulletSpeed;
    public float bulletSpread;
    public float bulletDamage;
    public float fireRate;
    public Transform[] firePos;
    public GameObject shootFX;
    private bool canFire = false;
    public int bulletArmorPenLevel;


    private Transform target;

    void Start()
    {
        // get the player
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);
        if (player != null) target = player.transform;
    }

    void Update()
    {
        if (target == null) return;

        if (CanSeeTarget())
        {
            LookAtTarget();
            Shoot();
        }
    }
    //i can seeeeee youuuuuu
    bool CanSeeTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        //check if in rang
        if (distanceToTarget <= detectionRange)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            //can i see you?
            if (!Physics.Linecast(transform.position, target.position, obstructionMask))
            {
                return true;
            }
        }
        return false;
    }

    void LookAtTarget()
    {
        //loookie lookie looo
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Vector3 currentEuler = transform.rotation.eulerAngles;
        Vector3 targetEuler = targetRotation.eulerAngles;

        float nextX = Mathf.MoveTowardsAngle(currentEuler.x, targetEuler.x, verticalSpeed * Time.deltaTime * 10f);
        float nextY = Mathf.MoveTowardsAngle(currentEuler.y, targetEuler.y, horizontalSpeed * Time.deltaTime * 10f);

        transform.rotation = Quaternion.Euler(nextX, nextY, 0);
    }

    void Shoot()
    {
        if (canFire)
        {
            foreach (Transform t in firePos)
            {
                //add bullet spread
                float randomX = Random.Range(-bulletSpread, bulletSpread);
                float randomY = Random.Range(-bulletSpread, bulletSpread);
                Quaternion spreadRotation = t.rotation * Quaternion.Euler(randomX, randomY, 0);

                //shootie mcshootshoot
                var bul = Instantiate(bullet, t.position, spreadRotation);
                bul.GetComponent<BulletScript>().shootBullet(bulletSpeed, bulletDamage, bulletArmorPenLevel);
                Instantiate(shootFX, t.position, t.rotation);
                canFire = false;
                Invoke(nameof(FireRateLimit), fireRate);
            }
            
        }
    }
    void FireRateLimit()
    {
        //limits the fire
        canFire = true;
    }
}
