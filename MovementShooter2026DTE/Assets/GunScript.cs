
using System.Collections;
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
    public GameObject bulletTrail;

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
    public float MagazineNaniteCost;

    [Header("Effects")]
    //shootingFX
    public GameObject[] barrelFX;
    public Transform barrelFXParent;
    //ShellFX
    public GameObject shell;
    public Transform shellEjectPoint;
    //Reloading FX
    public bool magazine = true;
    public GameObject MagazineModel;
    public GameObject staticMagazine;
    //Animations
    public Animator gunAnimator;
    //SoundFX
    public GameObject soundEffectGObj;
    public AudioClip shootingSFX;
    public AudioClip startedReloadingSFX;
    public AudioClip finishedReloadingSFX;
    public float volume;


    [Header("References")]
    public PlayerGunManager player;
    public Transform bulletPosition;
    public GameObject hitMark;

    [Header("UI")]
    public Sprite Icon; 
    public string UISubtext;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readyToFire = true;

        if (SpareMagazines > 0)
            UISubtext = $"{AmmoLoaded}/{maxAmmoLoaded} | {SpareMagazines} mag(s) left";
        else
            UISubtext = $"{AmmoLoaded}/{maxAmmoLoaded} | 0 mags left, ({player.reloadselectedGun.ToString()}) to create new (-{MagazineNaniteCost}N)";
    }
    public void selectGun()
    {
        readyToFire = false;
        gunAnimator.SetTrigger("Draw");
        Invoke(nameof(FireRateLimit), 0.5f);
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
                    var trail = Instantiate(bulletTrail, bulletPosition.position, Quaternion.Euler(bulletAngle));
                    trail.GetComponent<BulletTrailScript>().moveSpeed = BulletSpeed;
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
                var trail = Instantiate(bulletTrail, bulletPosition.position, Quaternion.Euler(bulletAngle));
                trail.GetComponent<BulletTrailScript>().moveSpeed = BulletSpeed;
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
            //SoundFX
            if (shootingSFX != null)
            {
                var s = Instantiate(soundEffectGObj, transform.position, Quaternion.identity);
                s.GetComponent<SoundEffectScript>().sound = shootingSFX;
                s.GetComponent<SoundEffectScript>().volume = volume;
            }

            readyToFire=false;
            AmmoLoaded--;

            if (SpareMagazines > 0)
                UISubtext = $"{AmmoLoaded}/{maxAmmoLoaded} | {SpareMagazines} mag(s) left";
            else
                UISubtext = $"{AmmoLoaded}/{maxAmmoLoaded} | 0 mags left, ({player.reloadselectedGun.ToString()}) to create new (-{MagazineNaniteCost}N)";


            if (AmmoLoaded == 0)
            {
                gunAnimator.SetBool("gunEmpty", true);
            }
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
        if (!reloading && SpareMagazines > 0)
        {
            //removes the magazine
            if (magazine)
                EjectMagazine();
            //makes sure the gun knows its reloading
            reloading = true;
            //changes the UI
            UISubtext = $"Reloading...";
            //Sound Effect
            if (startedReloadingSFX != null)
            {
                var s = Instantiate(soundEffectGObj, transform.position, Quaternion.identity);
                s.GetComponent<SoundEffectScript>().sound = startedReloadingSFX;
                s.GetComponent<SoundEffectScript>().volume = volume;
            }
            //Triggers the function after the time that reload speed is set to
            Invoke(nameof(reloadin), reloadSpeed);
        }
        else if (!reloading && SpareMagazines <= 0)
        {
            //Pretends like the guns reloading so the player doesint get confused 
            reloading = true;
            UISubtext = "Generating...";
            Invoke(nameof(RegenMagazine), reloadSpeed/2);
        }
    }

    private void reloadin()
    {
        SpareMagazines--;
        reloading = false;
        staticMagazine.SetActive(true);
        AmmoLoaded = maxAmmoLoaded;
        if (SpareMagazines > 0)
            UISubtext = $"{AmmoLoaded}/{maxAmmoLoaded} | {SpareMagazines} mag(s) left";
        else
            UISubtext = $"{AmmoLoaded}/{maxAmmoLoaded} | 0 mags left, ({player.reloadselectedGun.ToString()}) to create new (-{MagazineNaniteCost}N)" ;
        //Sound Effect
        if (finishedReloadingSFX != null)
        {
            var s = Instantiate(soundEffectGObj, transform.position, Quaternion.identity);
            s.GetComponent<SoundEffectScript>().sound = finishedReloadingSFX;
            s.GetComponent<SoundEffectScript>().volume = volume;
        }
        gunAnimator.SetBool("gunEmpty", false);
    }
   
    public void RegenMagazine()
    {
        if (player.Nanites > MagazineNaniteCost)
        {
            reloading = false;
            SpareMagazines++;
            player.Nanites-=MagazineNaniteCost;
            if (SpareMagazines > 0)
                UISubtext = $"{AmmoLoaded}/{maxAmmoLoaded} | {SpareMagazines} mag(s) left";
            else
                UISubtext = $"{AmmoLoaded}/{maxAmmoLoaded} | 0 mags left, ({player.reloadselectedGun.ToString()}) to create new (-{MagazineNaniteCost}N)";

        }
    }
    private void EjectShell()
    {
        var shellgo = Instantiate(shell, shellEjectPoint.transform);
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
