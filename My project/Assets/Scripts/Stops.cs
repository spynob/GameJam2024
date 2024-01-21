using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stops : MonoBehaviour
{
    public Vector3 StopPosition;

    private void Awake()
    {
        StopPosition = gameObject.transform.position;       
    }
}
