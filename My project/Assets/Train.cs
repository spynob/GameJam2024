using System;
using Unity.VisualScripting;
using UnityEngine;

public class Train : MonoBehaviour
{
    public Stops[] allStops; //Change to planets

    public const float cruiseAlt = 500; //Top altitude is half of what the number is
    private float t = 0;
    private float ForwardSpeed = 20f;
    private float time = 19.55f;
    private float increment = 0.1f;
    private int Steps = 1;

    // Start is called before the first frame update
    void Start()
    {
        allStops = FindObjectsOfType<Stops>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTrain();
    }

    public void RotateTrainTowardsTarget()
    {

    }

    public void LaunchTrain()
    {
        //Position of the target planet normalized
        Vector3 planetPos = new(28, 0, 31);
        planetPos.Normalize();
        //Current altitude
        float currentAlt = transform.position.y;
        //Percentage of travel done based on a bell curve
        float travelPercentage = (float)(1 / (0.4 * Math.Sqrt(2 * Math.PI)) * Math.Pow(Math.E, -(Math.Pow((t - 2), 2) / (2 * Math.Pow(0.4, 2)))));
        //Velocity vector 
        Vector3 Velocity = new(transform.position.x + ForwardSpeed * planetPos.x * Time.deltaTime, (cruiseAlt - currentAlt) * travelPercentage, transform.position.z + ForwardSpeed * planetPos.z * Time.deltaTime);
        //New position of our train
        transform.position = Velocity;
        //Increment the x variable of our bell curve based 
        t += increment * Time.deltaTime;
    }

    public void CruiseTrain()
    {

    }

    public void MoveTrain()
    {
        if (transform.position.y < cruiseAlt / 2 - 1 && Steps == 1)
        {
            LaunchTrain();
        }
        else
        {
            ++Steps; 
        }
    }
}
