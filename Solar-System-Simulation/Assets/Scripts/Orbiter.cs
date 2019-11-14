using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    public double OrbitTime = 365; // in seconds
    public Vector3 OrbitAxis = Vector3.up;
    public double X; // pos
    public double Y; // pos
    public double Z; // pos
    public Vector3d mPosition;

    // Start is called before the first frame update
    void Start()
    {
        mPosition = new Vector3d(X, Y, Z);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(gameObject.name + ": " + mPosition);
    }
}
