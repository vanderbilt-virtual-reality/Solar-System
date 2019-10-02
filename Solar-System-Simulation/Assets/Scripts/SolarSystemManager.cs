using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemManager : MonoBehaviour
{
    public float SizeScale = 1;
    public float TimeScale = 1;
    public GameObject SolarSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SolarSystem.transform.localScale = Vector3.one * SizeScale;
    }
}
