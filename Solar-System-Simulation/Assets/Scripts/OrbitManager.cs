using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // TODO: elliptical orbits
        foreach(Transform child in transform) {
            Orbiter o = child.gameObject.GetComponent<Orbiter>();
            if (o != null) {
                o.transform.RotateAround(
                    transform.position, 
                    o.OrbitAxis, 
                    (float) (FindObjectOfType<SolarSystemManager>().TimeScale * Time.deltaTime * 360 / o.OrbitTime)
                );
            }
        }
    }
}
