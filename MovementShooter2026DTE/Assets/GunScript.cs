
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [Header("Misc")]
    public bool gunSelected;
    public GameObject gunObj;

    public bool Debugging;


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
    public bool automatic = true;
    public bool shooting;
    private bool firerateTimerActive;

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

    [Header("Effects")]
    //shootingFX
    public GameObject[] barrelFX;
    public Transform barrelFXParent;
    //ShellFX
    public GameObject shell;
    public Transform shellEjectPoint;
    //Reloading FX
    public GameObject MagazineModel;
    public GameObject staticMagazine;
    //Animations
    public Animator gunAnimator;


    [Header("References")]
    public PlayerGunManager player;
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
            gunAnimator.SetTrigger("shoot");
            shooting = true;
            Vector3 bulletAngle = bulletPosition.forward;
            if (spread)
                bulletAngle += new Vector3(0, Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread));

            if (shotGun)
            {
                float damageDelt = 0f;
                Color col = Color.black;
                for (int i = 0; i < pellets; i++)
                {
                    bulletAngle += new Vector3(Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread/2, BulletSpread/2),0);
                    if (Physics.Raycast(bulletPosition.position, bulletAngle, out bullet, shootDistance, bulletHitLayer))
                    {
                        if (bullet.collider.gameObject.GetComponent<EnemyManager>() != null)
                        {
                            bullet.collider.gameObject.GetComponent<EnemyManager>().ChangeHealth(-BulletDamage, ArmorPenLevel);
                            if (bullet.collider.gameObject.GetComponent<EnemyManager>().armorLevel > ArmorPenLevel)
                            {
                                col = Color.red;
                                damageDelt += BulletDamage / 5;
                            }
                            else if (bullet.collider.gameObject.GetComponent<EnemyManager>().armorLevel == ArmorPenLevel)
                            {
                                col = Color.yellow;
                                damageDelt += BulletDamage / 5;
                            }
                            else
                            {
                                col = Color.white;
                                damageDelt += BulletDamage;
                            }
                            print("hit enemy");
                        }
                        Instantiate(hitMark, bullet.point, Quaternion.LookRotation(bullet.normal));
                        if (Debugging)
                            Debug.DrawRay(bulletPosition.position, bulletAngle * Vector3.Distance(bulletPosition.position, bullet.point), Color.green,1000f, true);
                    }
                    else
                    {
                        if (Debugging)
                            Debug.DrawRay(bulletPosition.position, bulletAngle, Color.red, 1000f, true);
                    }
                }
                
                if (damageDelt > 0)
                    player.addDamage(damageDelt, col);
            }
            else
            {
                if (Physics.Raycast(bulletPosition.position, bulletAngle, out bullet, shootDistance, bulletHitLayer))
                {
                    if (bullet.collider.gameObject.GetComponent<EnemyManager>() != null)
                    {
                        bullet.collider.gameObject.GetComponent<EnemyManager>().ChangeHealth(-BulletDamage, ArmorPenLevel);
                        Color col = Color.black;
                        float bulDamage = BulletDamage;
                        if (bullet.collider.gameObject.GetComponent<EnemyManager>().armorLevel > ArmorPenLevel)
                        {
                            col = Color.red;
                            bulDamage = BulletDamage / 5;
                        }
                        else if (bullet.collider.gameObject.GetComponent<EnemyManager>().armorLevel == ArmorPenLevel)
                        {
                            col = Color.yellow;
                            bulDamage = BulletDamage / 2;
                        }
                        else
                        {
                            col = Color.white;
                        }
                        player.addDamage(bulDamage, col);
                        print("hit enemy");
                    }
                    Instantiate(hitMark, bullet.point, Quaternion.LookRotation(bullet.normal));
                    print(bullet.collider.name + " " + bullet.point);
                    if (Debugging)
                        Debug.DrawRay(bulletPosition.position, bulletAngle*Vector3.Distance(bulletPosition.position, bullet.point), Color.green, 1000f, true);

                }
                else
                {
                    if (Debugging)
                        Debug.DrawRay(bulletPosition.position, bulletAngle, Color.red, 1000f, true);
                }
            }

            //Gun effect
            foreach (GameObject p in barrelFX)
            {
                Instantiate(p, barrelFXParent);
            }
            //ShellFX
            EjectShell();
            if (automatic && !firerateTimerActive)
            {
                firerateTimerActive = true;
                Invoke(nameof(FireRateLimit), FireRate);
            }

            readyToFire=false;
            AmmoLoaded--;
            UISubtext = AmmoLoaded + "/" + maxAmmoLoaded;
        }
    }
    public void stopShooting()
    {
        shooting = false;

        if (!automatic && !firerateTimerActive)
        {
            firerateTimerActive = true;
            Invoke(nameof(FireRateLimit), FireRate);
        }
            
    }

    public void Reload()
    {
        if (!reloading)
        {
            EjectMagazine();
            reloading = true;
            UISubtext = "Reloading...";
            Invoke(nameof(reloadin), reloadSpeed);
        }
    }
    private void reloadin()
    {
        SpareMagazines--;
        reloading = false;
        staticMagazine.SetActive(true);
        AmmoLoaded = maxAmmoLoaded;
        UISubtext = AmmoLoaded + "/" + maxAmmoLoaded;
    }

    private void EjectShell()
    {
        Instantiate(shell, shellEjectPoint.transform);
    }

    private void EjectMagazine()
    {
        Instantiate(MagazineModel, staticMagazine.transform.position, staticMagazine.transform.rotation);
        staticMagazine.SetActive(false);
    }
    public void FireRateLimit()
    {
        readyToFire = true;
        firerateTimerActive = false;
    }
}
