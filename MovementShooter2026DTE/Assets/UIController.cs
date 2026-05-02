using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Misc")]
    public bool debuggingMode;
    public GameObject UIGObj;

    [Header("Speedometer")]
    public float speed;
    public TMP_Text speedText;

    [Header("State")]
    public string state;
    public TMP_Text stateText;

    [Header("Guns")]
    public Sprite gunImage;
    public string Gunsubtext;
    public int gunIndex;
    public int gunCount;
    public Image nameText;
    public TMP_Text subtextText;

    [Header("Health")]
    public float nanitePercent;
    public TMP_Text nanitePercentText;
    public bool dead;
    public GameObject deadUI;

    [Header("Damage + Kill")]
    public TMP_Text damageIndicator;
    public float MaxdamageIndicatorTime;
    private float damageIndicatorTime;
    [Header("Mission")]
    public bool missionComplete;
    public GameController gameControl;
    public GameObject missionCompletePopup;
    public KeyCode restartGameKeycode;

    [Header("Menu")]
    public KeyCode menuKey;
    public GameObject menu;
    public bool menuActive;

    [Header("Gun Selection")]
    public bool selectionActive;
    public GameObject gunselectionGObj;
    

    private void Update()
    {
        speedText.text = Mathf.Round(speed).ToString() + "km/ph";
        if (debuggingMode)
        {
            stateText.text = state.ToString();
            stateText.gameObject.SetActive(true);
        }
        else
            stateText.gameObject.SetActive(false);

        nameText.sprite = gunImage;
        subtextText.text = Gunsubtext.ToString();
        nanitePercentText.text = "Health = " + nanitePercent.ToString() + "%";

        if (damageIndicatorTime > 0)
        {
            damageIndicatorTime -= Time.deltaTime;
        }


        if (Input.GetKey(restartGameKeycode) && missionComplete)
        {
            gameControl.RestartMission();
        }
        if (!selectionActive)
        {
            if (!dead)
            {
                if (Input.GetKeyDown(menuKey))
                {
                    menuActive = !menuActive;
                }
                if (menuActive)
                {
                    Time.timeScale = 0f;
                    menu.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    if (!dead)
                    {
                        Time.timeScale = 1f;
                        menu.SetActive(false);
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                }
            }
        }
        else
        {
            UIGObj.SetActive(false);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void closeMenu()
    {
        menuActive = false;
    }
    public void ChangeMenuStatus()
    {
        menuActive = !menuActive;
    }

    private void LateUpdate()
    {
        damageIndicator.gameObject.SetActive(damageIndicatorTime > 0);
    }
    public void ChangeDamageIndicator(float dam, Color col)
    {
        damageIndicatorTime = MaxdamageIndicatorTime;
        damageIndicator.text = dam.ToString();
        damageIndicator.color = col;
    }
    public void ActivateKillIndicator()
    {
        //removed
    }

    public void CompleteMission()
    {
        missionCompletePopup.SetActive(true);
        missionComplete = true;
    }

    public void closeGunSelectionMenu()
    {
        selectionActive = false;
        gunselectionGObj.SetActive(false);
        UIGObj.SetActive(true);
    }

    public void playerDied()
    {
        dead = true;
        deadUI.SetActive(true);
        Time.timeScale = 0.2f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
