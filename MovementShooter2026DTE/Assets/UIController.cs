using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Speedometer")]
    public float speed;
    public Text speedText;

    [Header("State")]
    public string state;
    public Text stateText;

    [Header("Guns")]
    public string Gunname;
    public string Gunsubtext;
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

    private void Update()
    {
        speedText.text = speed.ToString();
        stateText.text = state.ToString();
        nameText.text = Gunname.ToString();
        subtextText.text = Gunsubtext.ToString();
        nanitePercentText.text = (nanitePercent.ToString()) + "%";

        if (damageIndicatorTime > 0)
        {
            damageIndicatorTime -= Time.deltaTime;
        }
        if (killiconVisibilityTime > 0)
        {
            killiconVisibilityTime -= Time.deltaTime;
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

}
