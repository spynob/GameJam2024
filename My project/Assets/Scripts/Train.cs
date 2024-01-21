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
    private static float forwardSpeed = 200f; //Forward speed of the train
    private float time = 19.55f; //Time to accomplish launch and landing
    private int Steps = 1; //Steps inside moving train function 
    private float Distance = 0;
    private bool Triggered = true;

    // Start is called before the first frame update
    void Start()
    {
        //Obtain all reference to celestial bodies
        allCelestials = FindObjectsOfType<Celestials>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Triggered)
        {
            MoveTrain(allCelestials[1], allCelestials[0]);
        }
        else
        {
            CruiseSpeedTrain(allCelestials[1].transform.position);
        }
    }

    public void RotateTrainTowardsTarget(Vector3 TargetPlanetPos) //To finish
    {
        //Radian angle towards the planet
        float radianAngle = (float)Math.Atan2(TargetPlanetPos.x - transform.position.x, TargetPlanetPos.z - transform.position.z);
        //Degree angle equivalent to the radian
        float degreeAngle = radianAngle * (180 / Mathf.PI);
        //Quaternion equivalent of our degree angle
        Quaternion targetAngle = Quaternion.Euler(0, degreeAngle, 0);
        transform.rotation = targetAngle; 
    }

    public void LaunchAndLandingTrain(Vector3 TargetPlanetPos)
    {
        //Current Alt
        float currentAlt = transform.position.y;
        //Position of the target planet
        Vector3 planetPos = TargetPlanetPos;
        //Percentage of travel done based on a bell curve
        float travelPercentage = (float)(1 / (0.4 * Math.Sqrt(2 * Math.PI)) * Math.Pow(Math.E, -(Math.Pow((t - 2), 2) / (2 * Math.Pow(0.4, 2)))));
        //Velocity y vector 
        Vector3 Velocity = new(0, 0, 0);
        if (Steps == 1)
        {//Launching function
            Velocity = new(transform.position.x, (cruiseAlt - currentAlt) * travelPercentage + currentAlt, transform.position.z);
        }
        else
        {//Landing function 
            Velocity = new(transform.position.x, (cruiseAlt - currentAlt) * travelPercentage, transform.position.z);
        }
        //New position of our train
        transform.position = Velocity;
        TargetPlanetPos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, TargetPlanetPos, forwardSpeed * Time.deltaTime); 
        //Increment the x variable of our bell curve based 
        t += increment * Time.deltaTime;
        Debug.Log(t); 
    }

    public void CruiseSpeedTrain(Vector3 TargetPlanetPos)
    {
        Vector3 planetPos = TargetPlanetPos;
        planetPos.y = transform.position.y; 
        //Vector3 Velocity = new(transform.position.x + cruiseSpeed * Mathf.Cos(transform.rotation.y) * Time.deltaTime, transform.position.y, transform.position.z + cruiseSpeed * Mathf.Sin(transform.rotation.y) * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, planetPos, forwardSpeed * Time.deltaTime);
    }

    public void MoveTrain(Celestials TargetPlanet, Celestials OriginalPlanet)
    {

        if (Steps != 0)
        {
            RotateTrainTowardsTarget(TargetPlanet.transform.position); 
        }

        switch (Steps)
        {
            case 1:
                {
                    if (transform.position.y < cruiseAlt / 2 - 1)
                    {
                        LaunchAndLandingTrain(TargetPlanet.transform.position);
                    }
                    else
                    {
                        Distance = Mathf.Sqrt(Mathf.Pow(OriginalPlanet.transform.position.x - transform.position.x, 2) + Mathf.Pow(OriginalPlanet.transform.position.y + OriginalPlanet.sphereCollider.radius - transform.position.y, 2) + Mathf.Pow(OriginalPlanet.transform.position.z - transform.position.z, 2));
                        ++Steps;
                        t = 1.958f;
                    }
                    break;
                }
            case 2:
                {
                    if (Distance / 2 <= Mathf.Sqrt(Mathf.Pow(TargetPlanet.transform.position.x - transform.position.x, 2) + Mathf.Pow(TargetPlanet.transform.position.y + TargetPlanet.sphereCollider.radius - transform.position.y, 2) + Mathf.Pow(TargetPlanet.transform.position.z - transform.position.z, 2)) && Steps == 2)
                    {
                        CruiseSpeedTrain(TargetPlanet.transform.position);
                    }
                    else
                    {
                        landingAlt = TargetPlanet.transform.position.y + TargetPlanet.sphereCollider.radius;
                        ++Steps;
                    }
                    break;
                }
            case 3:
                {
                    if(transform.position.y > landingAlt)
                    {
                        LaunchAndLandingTrain(TargetPlanet.transform.position);
                    }
                    else
                    {
                        CruiseSpeedTrain(TargetPlanet.transform.position);
                        if(TargetPlanet.transform.position.x == transform.position.x && TargetPlanet.transform.position.z == transform.position.z)
                        {
                            Triggered = false;
                            t = 0;
                            Steps = 1;
                        }
                    }
                    break;
                }
        }

        //if (Steps == 0)
        //{
        //    RotateTrainTowardsTarget(TargetPlanet.transform.position);
        //    ++Steps;
        //}

        //if (transform.position.y < cruiseAlt / 2 - 1 && Steps == 1)
        //{

        //    LaunchAndLandingTrain(TargetPlanet.transform.position);
        //}
        //else if (Steps == 1)
        //{
        //    print("wot");
        //    Distance = Mathf.Sqrt(Mathf.Pow(OriginalPlanet.transform.position.x - transform.position.x, 2) + Mathf.Pow(OriginalPlanet.transform.position.y + OriginalPlanet.sphereCollider.radius - transform.position.y, 2) + Mathf.Pow(OriginalPlanet.transform.position.z - transform.position.z, 2));
        //    ++Steps;
        //   // print(Distance);
        //   // print(Mathf.Sqrt(Mathf.Pow(TargetPlanet.transform.position.x - transform.position.x, 2) + Mathf.Pow(TargetPlanet.transform.position.y + TargetPlanet.sphereCollider.radius - transform.position.y, 2) + Mathf.Pow(TargetPlanet.transform.position.z - transform.position.z, 2)));
        //}

        //if (Distance/2 <= Mathf.Sqrt(Mathf.Pow(TargetPlanet.transform.position.x - transform.position.x, 2) + Mathf.Pow(TargetPlanet.transform.position.y + TargetPlanet.sphereCollider.radius - transform.position.y, 2) + Mathf.Pow(TargetPlanet.transform.position.z - transform.position.z, 2)) && Steps == 2)
        //{
        //    Debug.Log("oh man"); 
        //    CruiseSpeedTrain(TargetPlanet.transform.position);
        //   // print(Mathf.Sqrt(Mathf.Pow(TargetPlanet.transform.position.x - transform.position.x, 2) + Mathf.Pow(TargetPlanet.transform.position.y + TargetPlanet.sphereCollider.radius - transform.position.y, 2) + Mathf.Pow(TargetPlanet.transform.position.z - transform.position.z, 2)));
        //}
        //else if (Steps == 2)
        //{
        //    landingAlt = TargetPlanet.transform.position.y + TargetPlanet.sphereCollider.radius + 100;
        //    ++Steps;
        //}

        //if (Steps == 3 && transform.position.y > landingAlt)
        //{
        //    Debug.Log("Landing alt: " + landingAlt);
        //    Debug.Log("Y position: " + transform.position.y);

        //    LaunchAndLandingTrain(TargetPlanet.transform.position);
        //}
        //else if(Steps == 3)
        //{
        //    Debug.Log("help");
        //    CruiseSpeedTrain(TargetPlanet.transform.position);
        //}
    }
}
