using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{



    [SerializeField]
    Transform[] waypoints;

    [SerializeField]
    float moveSpeed = 5f;

    //int waypointIndex = 0;
    int waypointIndex;

    // Start is called before the first frame update
    //void Start()
    //{
    //    waypointIndex = Random.Range(0, waypoints.Length);
    //    transform.position = waypoints[waypointIndex].transform.position;
    //}

    public GameObject Enemy;

    // Start is called before the first frame update
    void Start()
    {

        //if (FsmVariables.GlobalVariables.GetFsmInt("var_global_LevelCounter").Value > 1)

        //{

            //for (int i = 1; i < FsmVariables.GlobalVariables.GetFsmInt("var_global_LevelCounter").Value; i++)
            //{
            //    Instantiate(Enemy);
            //    //waypointIndex = Random.Range(0, waypoints.Length);
            //    //transform.position = waypoints[waypointIndex].transform.position;
            //}

        //}

        /*

                if (FsmVariables.GlobalVariables.GetFsmInt("var_global_LevelCounter").Value == 2)
        {
            Instantiate(Enemy);
            waypointIndex = Random.Range(0, waypoints.Length);
            transform.position = waypoints[waypointIndex].transform.position;
        }
        */
    }

    // Update is called once per frame
   // void Update()
    //{
        
    //}
}
