using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalBody : MonoBehaviour
{
    public float mass;
    public Vector3 initialVelocity;
    public Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = initialVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
