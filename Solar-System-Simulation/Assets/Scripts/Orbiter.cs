﻿using System.Collections;
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
    public Vector3d mScaledPosition;
    [SerializeField] private float closeToPlanetScale = 1000000;
    public float planetSizeScale;
    private SolarSystemManager SolarSystemManager;
    private CharacterMovement CharacterMovement;
    private float startShowingPlanet = 10000000000;

    // Start is called before the first frame update
    void Start()
    {
        SolarSystemManager = GameObject.FindObjectOfType<SolarSystemManager>();
        CharacterMovement = GameObject.FindObjectOfType<CharacterMovement>();
        mPosition = new Vector3d(X, Y, Z);
        planetSizeScale = startShowingPlanet / closeToPlanetScale;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        planetSizeScale = startShowingPlanet / closeToPlanetScale;

        if (Vector3d.Magnitude(mScaledPosition) < startShowingPlanet)
        {
            double ratio = Vector3d.Magnitude(mScaledPosition) / startShowingPlanet;

            //CharacterMovement.m_MoveSpeed *= (float) ratio;

            float sizeIncrease = (float) Mathd.Lerp(1, 2.5, 1-ratio);
            if (gameObject.name == "Sun")
            {
                transform.localScale = transform.localScale * planetSizeScale * sizeIncrease;
            }
            else
            {
                Transform child = transform.GetChild(0);
                child.localScale = child.localScale * planetSizeScale / SolarSystemManager.PlanetOnlyScale * sizeIncrease;
            }
        }
           

        // Debug.Log(gameObject.name + ": " + mPosition);
    }

    void FixedUpdate()
    {
        //Debug.Log(gameObject.name + ": " +  mScaledPosition.ToString() + " ; " + Vector3d.Magnitude(mScaledPosition));
        if (Vector3d.Magnitude(mScaledPosition) < startShowingPlanet)
        {
            // Show planet in front of us
            // find some way to lock on & leave
    
            Vector3d gamePosition = mScaledPosition / closeToPlanetScale;

            // limit how close we get to planet
            Debug.Log($"gamePosition: {gamePosition}");
            float limit = 2500;
            if (Vector3d.Magnitude(gamePosition) < limit)
            {
                gamePosition = gamePosition * limit / Vector3d.Magnitude(gamePosition); // rescale to 10m away
            }

            transform.position = new Vector3((float)gamePosition.x, (float) gamePosition.y,(float) gamePosition.z);
          
        }
        else
        {
            transform.position = new Vector3(0f, -100f, 0);
        }
    }
}
