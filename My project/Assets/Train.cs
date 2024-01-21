using System;
using UnityEngine;

public class Train : MonoBehaviour
{

    public float speed = 0.4f;
    public Stops[] allStops;

    private int CurrentStop = 0;
    private const float cruiseAlt = 100000;
    private int t = 0; 

    // Start is called before the first frame update
    void Start()
    {
        allStops = FindObjectsOfType<Stops>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < cruiseAlt - 10)
        {
            MoveTrain(allStops[CurrentStop].StopPosition);
        }
    }

    public void MoveTrain(Vector3 NextStop)
    {
        //Vector3 velocity = new(NextStop.x - transform.position.x, NextStop.y - transform.position.y, NextStop.z - transform.position.z);
        //velocity.Normalize();

        Vector3 test = new(t * Time.deltaTime, cruiseAlt * Mathf.Atan(Time.time), 0);

        //transform.rotation = Quaternion.LookRotation(velocity);
        transform.position = test * speed;

        ++t; 

        //if(Mathf.Abs(transform.position.x - NextStop.x) < 1 
        //    && Mathf.Abs(transform.position.y - NextStop.y) < 1 
        //    && Mathf.Abs(transform.position.z - NextStop.z) < 1)
        //{
        //    transform.position = NextStop; 
        //}

        //if (transform.position == NextStop)
        //{
        //    ++CurrentStop;
        //}
    }
}
