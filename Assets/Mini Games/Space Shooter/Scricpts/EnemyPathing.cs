using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] EnemyWaveConfig enemyWaveConfig;
    List<Transform> waypoints;

    int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = enemyWaveConfig.GetWayPoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    public void SetWaveConfig(EnemyWaveConfig enemyWaveConfig)
    {
        this.enemyWaveConfig = enemyWaveConfig;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = enemyWaveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
