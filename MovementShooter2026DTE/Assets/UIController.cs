using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Misc")]
    public bool debuggingMode;


    [Header("Speedometer")]
    public float speed;
    public Text speedText;

    [Header("State")]
    public string state;
    public Text stateText;

    [Header("Guns")]
    public string Gunname;
    public string Gunsubtext;
    public int gunIndex;
    public int gunCount;
    public Text nameText;
    public Text subtextText;

    [Header("Health")]
    public float nanitePercent;
    public Text nanitePercentText;

    [Header("Damage + Kill")]
    public Text damageIndicator;
    public float MaxdamageIndicatorTime;
    private float damageIndicatorTime;
    //Kill icon
    public GameObject killIcon;
    public float maxKillIconVisibilityTime;
    private float killiconVisibilityTime;

    [Header("Mission")]
    public bool missionComplete;
    public GameController gameControl;
    public GameObject missionCompletePopup;
    public KeyCode restartGameKeycode;

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

        nameText.text = $"{Gunname.ToString()} ({gunIndex}/{gunCount})";
        subtextText.text = Gunsubtext.ToString();
        nanitePercentText.text = "N = " + nanitePercent.ToString() + "%";

        if (damageIndicatorTime > 0)
        {
            damageIndicatorTime -= Time.deltaTime;
        }


        if (Input.GetKey(restartGameKeycode) && missionComplete)
        {
            gameControl.RestartMission();
        }
       
    }


    private void LateUpdate()
    {
        damageIndicator.gameObject.SetActive(damageIndicatorTime > 0);
        killIcon.SetActive(killiconVisibilityTime > 0);
    }
    public void ChangeDamageIndicator(float dam, Color col)
    {
        damageIndicatorTime = MaxdamageIndicatorTime;
        damageIndicator.text = dam.ToString();
        damageIndicator.color = col;
    }
    public void ActivateKillIndicator()
    {
        killiconVisibilityTime = maxKillIconVisibilityTime;
        killIcon.SetActive(true);
    }

    public void CompleteMission()
    {
        missionCompletePopup.SetActive(true);
        missionComplete = true;
    }

}
