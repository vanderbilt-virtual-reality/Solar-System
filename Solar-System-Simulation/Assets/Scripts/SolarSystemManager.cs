using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolarSystemManager : MonoBehaviour
{
    
    private struct PlanetScale {
        public GameObject go;
        public Vector3 scale;
    };

    public float SizeScale = 1;
    public float TimeScale = 1;
    public float PlanetOnlyScale = 1;
    public GameObject SolarSystem;
    private PlanetScale[] PlanetScales;
    private bool CollidersOn = true;

    // Start is called before the first frame update
    void Start()
    {
        PlanetScales = GameObject.FindGameObjectsWithTag("Planet").Select(go => new PlanetScale { go=go, scale=go.transform.localScale}).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        SolarSystem.transform.localScale = Vector3.one * SizeScale;

        if (PlanetOnlyScale > 1 && CollidersOn) {
            foreach(PlanetScale ps in PlanetScales)
            {
                ps.go.GetComponent<SphereCollider>().enabled = false;
                CollidersOn = false;
            }
        } else if (PlanetOnlyScale <= 1 && !CollidersOn) {
            foreach(PlanetScale ps in PlanetScales)
            {
                ps.go.GetComponent<SphereCollider>().enabled = true;
                CollidersOn = true;
            }
        }

        foreach(PlanetScale ps in PlanetScales)
        {
            ps.go.transform.localScale = ps.scale * PlanetOnlyScale;
        }
    }
}
