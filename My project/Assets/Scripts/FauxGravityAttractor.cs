using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{
    public float gravity = -10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attract(Transform body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized; //distance from center of the planet
        Vector3 bodyUp = body.up; //direction the body is currently facing, want the body to be facin gravityUp

        //force on rigid body that makes it always head towards the planet
        body.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation; //gives us the amount to rotate towards target rotation
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);                                                                                    //
    }
}