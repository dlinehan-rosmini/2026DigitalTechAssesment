using System.Collections;
using UnityEngine;

public class EnemyPatrolGroupHandler : MonoBehaviour
{
    public EnemyManager[] enemies;
    public Transform destination;

    public Vector3 patrolBounds;
    public float randomiseTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destination.position = new Vector3(transform.position.x + Random.Range(patrolBounds.x, -patrolBounds.x), 0f, transform.position.z + Random.Range(patrolBounds.z, -patrolBounds.z));
        foreach (EnemyManager enemy in enemies)
        {
            enemy.destination = destination;
        }
        StartCoroutine(patrolDestinationReset());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator patrolDestinationReset()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomiseTime);
            destination.position = new Vector3(transform.position.x + Random.Range(patrolBounds.x, -patrolBounds.x), 0f, transform.position.z + Random.Range(patrolBounds.z, -patrolBounds.z));
        }
    }
}
