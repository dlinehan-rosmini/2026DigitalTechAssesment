using UnityEngine;

public class MissionManager : MonoBehaviour
{

    [Header("Mission")]
    public MissionType currentMissionType;


    public enum MissionType
    {
        assassinate,
        capture,
        destroy,
        clearArea,
    }
    public Transform currentMissionLocation;
    public GameObject missionCanvas;
    public Vector3[] missionLocations;
    [Header ("Mission Type Variants")]
    public GameObject[] AssassinateObjectives;
    public GameObject[] CaptureObjectives;
    public GameObject[] DestroyObjectives;
    public GameObject[] ClearAreaObjectives;

    [Header("Enemy + Game completion")]
    public EnemyPatrolGroupHandler[] patrolGroups;
    public bool completeMission;
    public bool completedEnemyPatrols;
    public bool completedGame;

    [Header("References")]
    public UIController ui;


    private void Start()
    {
        //Placeholder -> add randomiser and player choice code here
        currentMissionType = MissionType.assassinate;
        
        MissionObjectiveHandler currentObj = null;
        GameObject MissionGameObject = null;
        if (currentMissionType == MissionType.assassinate)
            MissionGameObject = Instantiate(AssassinateObjectives[Random.Range(0, AssassinateObjectives.Length)], currentMissionLocation);

        if (currentMissionType == MissionType.capture)
            MissionGameObject = Instantiate(CaptureObjectives[Random.Range(0, CaptureObjectives.Length)], currentMissionLocation);

        if (currentMissionType == MissionType.destroy)
            MissionGameObject = Instantiate(DestroyObjectives[Random.Range(0, DestroyObjectives.Length)], currentMissionLocation);

        if (currentMissionType == MissionType.clearArea)
            MissionGameObject = Instantiate(ClearAreaObjectives[Random.Range(0, ClearAreaObjectives.Length)], currentMissionLocation);
        //Add other mission types here
        if (MissionGameObject != null)
            currentObj = MissionGameObject.GetComponent<MissionObjectiveHandler>();
        currentMissionLocation.position = missionLocations[Random.Range(0, missionLocations.Length)];
        currentMissionLocation.Rotate(0, Random.Range(0, 360), 0);

        if (currentObj != null)
        {
            currentObj.manangeah = this;
            currentObj.missionLocation = currentMissionLocation;
        }

    }

    public void CompleteObjective()
    {
        print("done");
        missionCanvas.SetActive(false);
        completeMission = true;
    }
}
