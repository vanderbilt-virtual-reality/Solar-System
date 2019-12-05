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
    public GameObject Character;
    public Orbiter[] orbiters;
    private CharacterMovement mCharacterMovement;
    private PlanetScale[] PlanetScales;
    private bool CollidersOn = true;

    // Start is called before the first frame update
    void Start()
    {
        orbiters = FindObjectsOfType<Orbiter>();
        Debug.Log(orbiters.Length);
        mCharacterMovement = Character.GetComponent<CharacterMovement>();
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

    void FixedUpdate()
    {
        foreach (Orbiter o in orbiters)
        {
        
            o.mScaledPosition = o.mPosition - mCharacterMovement.mPosition;
        }
        mCharacterMovement.mScaledPosition = new Vector3d(0d, 0d, 0d);
    }
}
