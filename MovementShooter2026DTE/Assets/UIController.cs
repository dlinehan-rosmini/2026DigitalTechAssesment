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
    public string nanitePercent;
    public Text nanitePercentText;

    private void Update()
    {
        speedText.text = speed.ToString();
        stateText.text = state.ToString();
        nameText.text = Gunname.ToString();
        subtextText.text = Gunsubtext.ToString();
        nanitePercentText.text = nanitePercent.ToString();
    }
}
