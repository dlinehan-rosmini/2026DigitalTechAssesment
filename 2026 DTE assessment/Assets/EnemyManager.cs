using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header ("Vitality")]
    public float Health;
    public float FullHealth;
    public int armorLevel;

    [Header("Canvas Elements")]
    public Image healthBar;
    private float healthbarFillAmount;

    private void Start()
    {
        healthbarFillAmount = (Health / FullHealth);
        UpdateCanvas();
    }

    public void UpdateCanvas()
    {
        healthBar.fillAmount = healthbarFillAmount;
    }

    public void ChangeHealth(float amount, int ArmorLevel)
    {
        if (amount > 1)
        {
            if ((Health + amount) > FullHealth)
                Health += amount;
            else
                Health = FullHealth;
        }
        else
        {
            if (ArmorLevel > armorLevel)
            {
                Health += amount;
                print(amount);
            }
            else if (ArmorLevel == armorLevel)
            {
                Health += amount / 2;
                print(amount / 2);
            }
            else
            {
                Health += amount / 5;
                print(amount / 5);
            }
        }

        healthbarFillAmount = (Health / FullHealth);
        print(healthbarFillAmount);
        UpdateCanvas();
    }
}
