using UnityEngine;

public class PlayerGunManager : MonoBehaviour
{
    [Header ("Gun Selection Management")]
    public GunScript currentlySelectedGun;
    public int currentlySelectedGunIndex;

    [Header("References")]
    public PlayerMovement pm;
    public GunScript[] AvailableGuns;
    public GunScript[] guns;
    public UIController uiControl;

    [Header("Keybinds")]
    public KeyCode changeWeapon;
    public KeyCode shootGun;
    public KeyCode reloadselectedGun;
    public KeyCode takeDamage;
    public KeyCode throwThrowable;

    [Header("Health")]
    public float Nanites;
    public float fullNanites;
    public float maxNanites;
    public float nanitePercent;
    public int ArmorLevel;
    private int currentArmorLevel;

    [Header("Throwables")]
    public GameObject selectedThrowable;
    public GameObject[] throwables;
    public float throwDistance;
    public Transform throwPosition;
    public int throwableAmmo;
    public int maxThrowableAmmo;


    private void Start()
    {
        //test
        createLoadout(0, 2, 4);
        

        nanitePercent = Nanites / fullNanites;

        if ((nanitePercent * 100) > 200)
        {
            currentArmorLevel = ArmorLevel + 2;
        }
        else if ((nanitePercent * 100) > 100)
        {
            currentArmorLevel = ArmorLevel + 1;
        }
        else
        {
            currentArmorLevel = ArmorLevel;
        }

        uiControl.nanitePercent = nanitePercent * 100;
        currentlySelectedGun = AvailableGuns[currentlySelectedGunIndex];
        foreach (GunScript gun in AvailableGuns)
        {
            gun.gameObject.SetActive(false);
        }
        currentlySelectedGun.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(shootGun) && currentlySelectedGun != null)
            currentlySelectedGun.Shoot();

        if (Input.GetKeyUp(shootGun) && currentlySelectedGun != null)
            currentlySelectedGun.stopShooting();

        if (Input.GetKeyDown(reloadselectedGun) && currentlySelectedGun != null)
            currentlySelectedGun.Reload();

        if (Input.GetKeyDown(changeWeapon))
            changeSelectedWeapon();

        if (Input.GetKeyDown(takeDamage))
            ChangeHealth(-13, 10);

        if (Input.GetKeyDown(throwThrowable) && throwableAmmo > 0)
            Throwable();


        uiControl.Gunname = currentlySelectedGun.UIName;
        uiControl.Gunsubtext = currentlySelectedGun.UISubtext;

    }

    public void changeSelectedWeapon()
    {
        if (currentlySelectedGunIndex == 2)
        {
            currentlySelectedGunIndex = 0;
        }
        else
        {
            currentlySelectedGunIndex++;
        }
        foreach (GunScript gun in AvailableGuns)
        {
            gun.gameObject.SetActive(false);
        }
        currentlySelectedGun = AvailableGuns[currentlySelectedGunIndex];
        currentlySelectedGun.gameObject.SetActive(true);
        currentlySelectedGun.selectGun();
    }
    public void Throwable()
    {
        throwableAmmo--;
        var t = Instantiate(selectedThrowable, throwPosition.position, throwPosition.rotation);
        t.GetComponent<ThrowableScript>().Throw(throwDistance, throwPosition);


    }
    public void createLoadout(int firstGunIndex, int secondGunIndex, int thirdGunIndex)
    {
        AvailableGuns[0] = guns[firstGunIndex];
        AvailableGuns[1] = guns[secondGunIndex];
        AvailableGuns[2] = guns[thirdGunIndex];
    }

    public void ChangeHealth(float amount, int armor)
    {
        if (armor > currentArmorLevel)
        {

            Nanites += amount;
        }
        else if (armor == currentArmorLevel)
        {

            Nanites += amount/2;
        }
        else
        {
            Nanites += amount/5;
        }

        if (Nanites < 0)
        {
            Nanites = 0;
        }
        nanitePercent = Nanites / fullNanites;
        print(nanitePercent);
        uiControl.nanitePercent = nanitePercent * 100;
        if ((nanitePercent*100) > 200)
        {
            currentArmorLevel = ArmorLevel + 2;
        }
        else if ((nanitePercent * 100) > 100)
        {
            currentArmorLevel = ArmorLevel + 1;
        }
        else
        {
            currentArmorLevel = ArmorLevel;
        }
    }


    public void addDamage(float am, Color col)
    {
        uiControl.ChangeDamageIndicator(am, col);
    }
}
