using UnityEngine;
using UnityEngine.UI;

public class MissionObjectiveHandler : MonoBehaviour
{
    [Header("Mission")]
    public bool[] missionCriticalObjectives;
    public bool missionComplete;


    [Header("References")]
    public Transform missionLocation;
    public MissionManager manangeah;
    public Image missionImage;


    private void Update()
    {
        bool allComplete = true;
        foreach (bool step in missionCriticalObjectives)
        {
            allComplete = step;
            if (allComplete == false)
            {
                break;
            }
        }

        if (allComplete == true && missionComplete == false)
        {
            missionComplete = true;
            manangeah.CompleteObjective();
        }
    }
}
