using UnityEngine;

public class GunScript : MonoBehaviour
{
    [Header("Misc")]
    public bool gunSelected;
    public GameObject gunObj;


    [Header("Gun Variables")]
    public float BulletSpeed;
    public float BulletDamage;
    public float FireRate;
    private bool readyToFire;
    public float BulletSpread;
    public bool spread;
    private RaycastHit bullet;
    public float shootDistance;
    public LayerMask bulletHitLayer;

    [Header("Shotgun")]
    public bool shotGun;
    public int pellets;

    [Header("Ammo")]
    public int AmmoLoaded;
    public int maxAmmoLoaded;
    public int SpareMagazines;
    public float reloadSpeed;
    private bool reloading;
    public int ArmorPenLevel;

    [Header("References")]
    public PlayerMovement player;
    public Transform bulletPosition;
    public GameObject hitMark;

    [Header ("UI")]
    public string UIName;
    public string UISubtext;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readyToFire = true;

        UISubtext = AmmoLoaded + "/" + maxAmmoLoaded;
    }

    public void Shoot()
    {
        if (readyToFire && AmmoLoaded > 0 && !reloading)
        {
            Vector3 bulletAngle = bulletPosition.forward;
            if (spread)
                bulletAngle += new Vector3(0, Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread));

            if (shotGun)
            {
                for (int i = 0; i < pellets; i++)
                {
                    bulletAngle += new Vector3(Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread/2, BulletSpread/2),0);
                    if (Physics.Raycast(bulletPosition.position, bulletAngle, out bullet, shootDistance, bulletHitLayer))
                    {
                        if (bullet.collider.gameObject.GetComponent<EnemyManager>() != null)
                        {
                            bullet.collider.gameObject.GetComponent<EnemyManager>().ChangeHealth(-BulletDamage, ArmorPenLevel);
                            print("hit enemy");
                        }
                        Instantiate(hitMark, bullet.point, Quaternion.identity);
                        Debug.DrawRay(bulletPosition.position, bulletAngle, Color.green,1000f);
                    }
                    else
                    {
                        Debug.DrawRay(bulletPosition.position, bulletAngle, Color.red, 1000f);
                    }
                }
                    
            }
            else
            {
                if (Physics.Raycast(bulletPosition.position, bulletAngle, out bullet, shootDistance, bulletHitLayer))
                {
                    if (bullet.collider.gameObject.GetComponent<EnemyManager>() != null)
                    {
                        bullet.collider.gameObject.GetComponent<EnemyManager>().ChangeHealth(-BulletDamage, ArmorPenLevel);
                        print("hit enemy");
                    }
                    Instantiate(hitMark, bullet.point, Quaternion.identity);
                    print(bullet.collider.name + " " + bullet.point);
                    Debug.DrawRay(bulletPosition.position, bulletAngle, Color.green);
                }
            }

            //pew pew
            
            Invoke(nameof(FireRateLimit), FireRate);
            readyToFire=false;
            AmmoLoaded--;
            UISubtext = AmmoLoaded + "/" + maxAmmoLoaded;
        }
        
    }


    public void Reload()
    {
        if (!reloading)
        {
            reloading = true;
            UISubtext = "Reloading...";
            Invoke(nameof(reloadin), reloadSpeed);
        }
    }
    private void reloadin()
    {
        SpareMagazines--;
        reloading = false;
        AmmoLoaded = maxAmmoLoaded;
        UISubtext = AmmoLoaded + "/" + maxAmmoLoaded;
    }


    public void FireRateLimit()
    {
        readyToFire = true;
    }
}
