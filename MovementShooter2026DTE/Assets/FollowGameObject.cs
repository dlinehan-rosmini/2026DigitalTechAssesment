using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    public bool matchY;
    public Transform gObjToFollow;
    public Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        float y = 0f;
        if (matchY)
            y = gObjToFollow.position.y + offset.y;
        else
            y = offset.y;
        Vector3 v = new Vector3(gObjToFollow.position.x + offset.x, y, gObjToFollow.position.z + offset.z);
    }
}
