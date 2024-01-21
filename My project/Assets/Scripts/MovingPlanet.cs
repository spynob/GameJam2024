using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MovingPlanet : MonoBehaviour
{
    [SerializeField] public float excentricity;
    [SerializeField] public float a;
    private float b;
    [SerializeField] public float angularVelocity;
    private int t = 0;
    [SerializeField] public int initialPosition;
    [SerializeField] public int radius;
    private float centerx;
    private float centerz;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(a * (float)Math.Cos(angularVelocity * t) + centerx, 0.0f, b * (float)Math.Sin(angularVelocity * t) + centerz);
        t++;

    }

    public void InitializeAttr(float pA)
    {
        this.a = pA;
        this.b = a * (float)Math.Sqrt(1 - Math.Pow(excentricity, 2));
        this.centerx = a * excentricity;
    }
}
