using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int numberOfPlanets = 10;
    public GameObject planetPrefab;
    private float minSpeed = 0.0001f;
    private float maxSpeed = 0.005f;
    private int scaleFactor = 10;

    //Random random = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfPlanets; i++)
        {
            GameObject planet = Instantiate(planetPrefab, new Vector3(500 + i * 450, 0, 500 + i * 350), Quaternion.identity);
            Rigidbody pBody = planet.GetComponent<Rigidbody>();
            pBody.mass = Random.Range(1, 30);
            planet.transform.localScale = new Vector3(scaleFactor * pBody.mass, scaleFactor * pBody.mass, scaleFactor * pBody.mass);
            //float speed = (float)random.NextDouble() * (maxSpeed - minSpeed) + minSpeed;
            //p.angularVelocity = speed;
            //p.initialPosition = random.Next(0, Math.PI / speed);
            //p.excentricity = (float)random.NextDouble();
            //p.InitializeAttr(random.Next(i * 35, i * 35 + 15));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
