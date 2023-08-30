using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsMove : MonoBehaviour
{

    [SerializeField]
    Transform[] waypoints;

    [SerializeField]
    float moveSpeed = 5f;

    //int waypointIndex = 0;
    int waypointIndex;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = Random.Range(0, waypoints.Length);
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pointsmove();
    }

    void pointsmove()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, moveSpeed * Time.deltaTime);

        if (transform.position == waypoints [waypointIndex].transform.position)
        {
            //waypointIndex += 1;
            waypointIndex = Random.Range(0, waypoints.Length);
        }

        if (waypointIndex == waypoints.Length)
            waypointIndex = 0;
    }
}
