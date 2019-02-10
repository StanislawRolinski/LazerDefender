using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> enemyWaypoints;
    

    int wayPointIndex = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        enemyWaypoints = waveConfig.GetWaypoints();
        transform.position = enemyWaypoints[wayPointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }
    public WaveConfig WaveConfig { set => waveConfig = value; }
    private void Move()
    {
        if (wayPointIndex < enemyWaypoints.Count)
        {
            var targetPos = enemyWaypoints[wayPointIndex].transform.position;
            var movementThisFrame = waveConfig.MoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, movementThisFrame);
            if (transform.position == targetPos)
            {
                wayPointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
