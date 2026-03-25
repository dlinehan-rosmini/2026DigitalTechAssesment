using Pathfinding;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Vitality")]
    public bool dead;
    public float Health;
    public float FullHealth;
    public int armorLevel;

    [Header("Model")]
    public Animator soldierAnimator;
    public bool moving;
    public bool shooting;

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
    public bool debuggingMode;
    public GameObject model;
    public GameObject canvas;
    public CapsuleCollider collision;
    public UIController uicontrol;
    public PlayerGunManager playerGunManager;

    [Header ("Mission Objective")]
    public bool partOfObjective;
    public MissionObjectiveHandler misObjHandler;
    public int handlerIndex;

    [Header("Targeting + Attacking")]
    public bool canTargetPlayer;
    public bool canSeePlayer;

    public float viewAngle;
    public float viewDistance;

    public float checkInterval = 0.2f;
    private float nextCheckTime;

    public LayerMask targetingHitLayer;
    public Transform firePos;
    public GameObject shootFX;
    public bool canFire;
    public float fireRate;
    public float bulletDamage;
    public int ArmorPenLevel;
    public float bulletSpread;
    public GameObject bullet;
    public float bulletSpeed;

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
        playerGunManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGunManager>();
    }

    public void UpdateCanvas()
    {
        healthBar.fillAmount = healthbarFillAmount;
    }
    private void Update()
    {
        if (!shooting)
            moving = AIpath.velocity.magnitude > .4;

        AIpath.canMove = canMove;
        if (!shooting)
        {
            canMove = true;
        }
        else
        {
            moving = false;
            canMove = false;
        }
        if (soldierAnimator != null)
        {
            soldierAnimator.SetBool("shooting", shooting);
            soldierAnimator.SetBool("running", moving);
        }
    }
    private void FixedUpdate()
    {
        if (Time.time > nextCheckTime)
        {
            CheckIfLOSPLayer(); 
            nextCheckTime = Time.time + checkInterval;
        }
    }

    public void CheckIfLOSPLayer()
    {
        if (canTargetPlayer)
        {
            Vector3 directionToPlayer = playerGunManager.transform.position + new Vector3(0,0.5f,0)- firePos.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer <= viewDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(firePos.position, directionToPlayer, out hit, viewDistance,targetingHitLayer))
                {
                    if (hit.collider.gameObject.GetComponent<PlayerGunManager>() != null)
                    {
                        //pew pew pewww
                        Shoot(directionToPlayer);
                        shooting = true;
                        // Rotate the soldier model
                        directionToPlayer.y = 0; 
                        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, Time.deltaTime * 10);
                        //Debugging
                        canSeePlayer = true;
                        if (debuggingMode)
                            Debug.DrawRay(firePos.position, directionToPlayer * Vector3.Distance(firePos.position, hit.point), Color.green, 1f);
                    }
                    else
                    {
                        canSeePlayer = false;
                        shooting = false;
                        //Reset soldier's rotation
                        directionToPlayer.y = 0; 
                        Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
                        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, Time.deltaTime * 10);
                        if (debuggingMode)
                            Debug.DrawRay(firePos.position, directionToPlayer * Vector3.Distance(firePos.position, hit.point), Color.red, 1f);
                    }
                }
            }

        }
       
    }
    public void Shoot(Vector3 direction)
    {
        if (canFire)
        {
            //Convert the direction to the player into a confusing stupid quaterinisininfosnd
            Quaternion baseRotation = Quaternion.LookRotation(direction);
            //get random numbers for bullet spread
            float randomX = Random.Range(-bulletSpread, bulletSpread);
            float randomY = Random.Range(-bulletSpread, bulletSpread);
            //turn those random numbers into ANOTHER QUATERNIONNNNN
            Quaternion spreadRotation = Quaternion.Euler(randomX, randomY, 0);
            //this better be the last one
            Quaternion finalRotation = baseRotation * spreadRotation;
            //shoot bullet with calculations done before
            var bul = Instantiate(bullet, firePos.position, finalRotation);
            bul.GetComponent<BulletScript>().shootBullet(bulletSpeed, bulletDamage, ArmorPenLevel);
            Instantiate(shootFX, firePos.position, finalRotation);
            canFire = false;
            Invoke(nameof(resetFireRate), fireRate);
        }
    }
   
    private void resetFireRate()
    {
        canFire = true;
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
        canTargetPlayer = false;
        AIpath.canMove = canMove;
        canvas.SetActive(false);
        collision.enabled = false;
        model.transform.Rotate(new Vector3(model.transform.rotation.x, model.transform.rotation.y, 90f));
        uicontrol.ActivateKillIndicator();

        if (partOfObjective)
        {
            misObjHandler.missionCriticalObjectives[handlerIndex] = true;
        }
        Destroy(gameObject, 10f);


    }
}
