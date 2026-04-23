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
        Vector3 patDestination = new Vector3(transform.position.x + Random.Range(patrolBounds.x, -patrolBounds.x), 0f, transform.position.z + Random.Range(patrolBounds.z, -patrolBounds.z));
        foreach (EnemyManager enemy in enemies)
        {
            enemy.patrolDestination = new Vector3(patDestination.x + Random.Range(-5,5), 0f, patDestination.z + Random.Range(-5, 5));
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
            Vector3 patDestination = new Vector3(transform.position.x + Random.Range(patrolBounds.x, -patrolBounds.x), 0f, transform.position.z + Random.Range(patrolBounds.z, -patrolBounds.z));
            foreach (EnemyManager enemy in enemies)
            {
                enemy.patrolDestination = new Vector3(patDestination.x + Random.Range(-5, 5), 0f, patDestination.z + Random.Range(-5, 5));
            }
        }
    }
}
