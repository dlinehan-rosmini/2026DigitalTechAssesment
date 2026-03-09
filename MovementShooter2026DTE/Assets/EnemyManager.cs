using Pathfinding;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Vitality")]
    public bool dead;
    public float Health;
    public float FullHealth;
    public int armorLevel;

    [Header("Canvas Elements")]
    public Image healthBar;
    private float healthbarFillAmount;

    [Header("Pathfinding")]
    public float moveSpeed;
    public float rotateSpeed;
    public bool canMove;
    private AIDestinationSetter pathfinderDestination;
    private AIPath AIpath;
    public Transform destination;

    [Header("References")]
    public GameObject model;
    public GameObject canvas;
    public CapsuleCollider collision;
    public UIController uicontrol;

    private void Start()
    {
        uicontrol = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        AIpath = GetComponent<AIPath>();
        pathfinderDestination = GetComponent<AIDestinationSetter>();
        healthbarFillAmount = (Health / FullHealth);
        UpdateCanvas();
        pathfinderDestination.target = destination;
        AIpath.maxSpeed = moveSpeed;
        AIpath.rotationSpeed = rotateSpeed;
        AIpath.canMove = canMove;
        
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

        if (Health <= 0)
        {
            die();
        }

        healthbarFillAmount = (Health / FullHealth);
        print(healthbarFillAmount);
        UpdateCanvas();
    }

    private void die()
    {
        Health = 0;
        dead = true;
        canMove = false;
        AIpath.canMove = canMove;
        canvas.SetActive(false);
        collision.enabled = false;
        model.transform.Rotate(new Vector3(model.transform.rotation.x, model.transform.rotation.y, 90f));
        uicontrol.ActivateKillIndicator();
        Destroy(gameObject, 10f);
    }
}
