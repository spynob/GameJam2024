using System;
using Unity.VisualScripting;
using UnityEngine;
//Sphere collider n'a pas de radius set après avoir créer les planètes dans le spawner uniquement le scale est changé
//Le train snap up à la cruiseAlt la deuxième fois que la fonction est appelée
//Faut tweak les values 
//Pis il faut actually land 
public class Train : MonoBehaviour
{
    public Celestials[] allCelestials; //All planets reference

    public const float cruiseAlt = 500; //Top altitude is half of what the number is
    private float landingAlt = 0; //Ground level of the target planet 
    private float t = 0; //X valuie of the bell curve
    private float increment = 0.4f; //Increment to the x value of the bell curve
    private static float forwardSpeed = 150f; //Forward speed of the train
    private float cruiseSpeed = forwardSpeed * 4; 
    private float time = 19.55f; //Time to accomplish launch and landing
    private int Steps = 0; //Steps inside moving train function 

    private float Distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Obtain all reference to celestial bodies
        allCelestials = FindObjectsOfType<Celestials>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTrain(allCelestials[1], allCelestials[0]);
    }

    public void RotateTrainTowardsTarget(Vector3 TargetPlanetPos) //To finish
    {
        float radianAngle = (float)Math.Atan2(TargetPlanetPos.x - transform.position.x, TargetPlanetPos.z - transform.position.z);
        float degreeAngle = radianAngle * (180 / Mathf.PI);
        Quaternion targetAngle = Quaternion.Euler(0, degreeAngle, 0);
        //float test = transform.rotation.y - degreeAngle;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, 5.0f);
        //print(degreeAngle);
    }

    public void LaunchAndLandingTrain(Vector3 TargetPlanetPos, float Alt)
    {
        //Position of the target planet
        Vector3 planetPos = TargetPlanetPos;
        //Percentage of travel done based on a bell curve
        float travelPercentage = (float)(1 / (0.4 * Math.Sqrt(2 * Math.PI)) * Math.Pow(Math.E, -(Math.Pow((t - 2), 2) / (2 * Math.Pow(0.4, 2)))));
        //Velocity y vector 
        Vector3 Velocity = new(transform.position.x, (cruiseAlt - Alt) * travelPercentage, transform.position.z);
        //New position of our train
        transform.position = Velocity;
        TargetPlanetPos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, TargetPlanetPos, forwardSpeed * Time.deltaTime); 
        //Increment the x variable of our bell curve based 
        t += increment * Time.deltaTime;
    }

    public void CruiseSpeedTrain(Vector3 TargetPlanetPos)
    {
        Vector3 planetPos = TargetPlanetPos;
        //Vector3 Velocity = new(transform.position.x + cruiseSpeed * Mathf.Cos(transform.rotation.y) * Time.deltaTime, transform.position.y, transform.position.z + cruiseSpeed * Mathf.Sin(transform.rotation.y) * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, planetPos, forwardSpeed * Time.deltaTime);
    }

    public void MoveTrain(Celestials TargetPlanet, Celestials OriginalPlanet)
    {

        if (Steps != 0)
        {
            float radianAngle = (float)Math.Atan2(TargetPlanet.transform.position.x - transform.position.x, TargetPlanet.transform.position.z - transform.position.z);
            float degreeAngle = radianAngle * (180 / Mathf.PI);
            Quaternion targetAngle = Quaternion.Euler(0, degreeAngle, 0);
            transform.rotation = targetAngle;
        }

        if (Steps == 0)
        {
            RotateTrainTowardsTarget(TargetPlanet.transform.position);
            ++Steps;
        }

        if (transform.position.y < cruiseAlt / 2 - 1 && Steps == 1)
        {
            //Current altitude
            float currentAlt = transform.position.y;

            LaunchAndLandingTrain(TargetPlanet.transform.position, currentAlt);
        }
        else if (Steps == 1)
        {
            Distance = Mathf.Sqrt(Mathf.Pow(OriginalPlanet.transform.position.x - transform.position.x, 2) + Mathf.Pow(OriginalPlanet.transform.position.y + OriginalPlanet.sphereCollider.radius - transform.position.y, 2) + Mathf.Pow(OriginalPlanet.transform.position.z - transform.position.z, 2));
            ++Steps;
           // print(Distance);
           // print(Mathf.Sqrt(Mathf.Pow(TargetPlanet.transform.position.x - transform.position.x, 2) + Mathf.Pow(TargetPlanet.transform.position.y + TargetPlanet.sphereCollider.radius - transform.position.y, 2) + Mathf.Pow(TargetPlanet.transform.position.z - transform.position.z, 2)));
        }

        if (Distance/2 <= Mathf.Sqrt(Mathf.Pow(TargetPlanet.transform.position.x - transform.position.x, 2) + Mathf.Pow(TargetPlanet.transform.position.y + TargetPlanet.sphereCollider.radius - transform.position.y, 2) + Mathf.Pow(TargetPlanet.transform.position.z - transform.position.z, 2)) && Steps == 2)
        {
            CruiseSpeedTrain(TargetPlanet.transform.position);
           // print(Mathf.Sqrt(Mathf.Pow(TargetPlanet.transform.position.x - transform.position.x, 2) + Mathf.Pow(TargetPlanet.transform.position.y + TargetPlanet.sphereCollider.radius - transform.position.y, 2) + Mathf.Pow(TargetPlanet.transform.position.z - transform.position.z, 2)));
        }
        else if (Steps == 2)
        {
            landingAlt = TargetPlanet.transform.position.y + TargetPlanet.sphereCollider.radius;
            Debug.Log("bruh");
            ++Steps;
        }

        if (Steps == 3 && transform.position.y > landingAlt)
        {
            float currentAlt = transform.position.y; //Comprends pas pourquoi tu snap plus haut qu'earlier en plus 
            LaunchAndLandingTrain(TargetPlanet.transform.position, currentAlt);
        }
        else
        {
            Debug.Log("help");
            CruiseSpeedTrain(TargetPlanet.transform.position);
        }
    }
}
