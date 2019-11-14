using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockOrbiter : MonoBehaviour
{
    public GameObject ReferenceObject;
    void Start()
    {
        Transform SolarSystemTransform = GameObject.Find("SolarSystem").transform;
        string searchString = gameObject.name.Substring(4);

        if (searchString == "Sun")
        {
            ReferenceObject = SolarSystemTransform.Find(searchString).gameObject;
        } else
        {
            ReferenceObject = SolarSystemTransform.Find(searchString + "System").gameObject;
        }
    }
}
