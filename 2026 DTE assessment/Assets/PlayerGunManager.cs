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

    [Header("Health")]
    public float Nanites;
    public float fullNanites;
    public float maxNanites;
    public float nanitePercent;
    public int ArmorLevel;


    private void Start()
    {
        //test
        createLoadout(0, 1, 3);

        nanitePercent = (Nanites / fullNanites) * 100;
        uiControl.nanitePercent = nanitePercent.ToString();
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

        if (Input.GetKeyDown(reloadselectedGun) && currentlySelectedGun != null)
            currentlySelectedGun.Reload();

        if (Input.GetKeyDown(changeWeapon))
            changeSelectedWeapon();

        if (Input.GetKeyDown(takeDamage))
            ChangeHealth(-50, 10);

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
    }

    public void createLoadout(int firstGunIndex, int secondGunIndex, int thirdGunIndex)
    {
        AvailableGuns[0] = guns[firstGunIndex];
        AvailableGuns[1] = guns[secondGunIndex];
        AvailableGuns[2] = guns[thirdGunIndex];
    }

    public void ChangeHealth(float amount, int armor)
    {
        if (amount > 0)
        {
            if ((Nanites + amount) > maxNanites)
                Nanites = maxNanites;
            else
                Nanites += amount;
        }
        else
        {
            if (armor > ArmorLevel)
            {
                Nanites += amount;
                print(amount);
            }
            else if (armor == ArmorLevel)
            {
                Nanites += amount / 2;
                print(amount / 2);
            }
            else
            {
                Nanites += amount / 5;
                print(amount / 5);
            }

        }




        if ((Nanites += amount) <= maxNanites || (Nanites += amount) > 0)
        {

            Nanites += amount;
        }
        else
            if ((Nanites += amount) > maxNanites)
            Nanites = maxNanites;
        else
            Nanites = 0;
        print(Nanites / fullNanites);
        nanitePercent = (Nanites / fullNanites) * 100;
        uiControl.nanitePercent = nanitePercent.ToString();
    }
}
