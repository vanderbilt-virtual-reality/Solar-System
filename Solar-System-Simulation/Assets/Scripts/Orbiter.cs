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
    public Vector3d mScaledPosition;
    [SerializeField] private float closeToPlanetScale = 1000000;
    public float planetSizeScale;
    private SolarSystemManager SolarSystemManager;

    // Start is called before the first frame update
    void Start()
    {
        SolarSystemManager = GameObject.FindObjectOfType<SolarSystemManager>();
        mPosition = new Vector3d(X, Y, Z);
        planetSizeScale = 100000000 / closeToPlanetScale;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Debug.Log("here");
        planetSizeScale = 100000000 / closeToPlanetScale;

        if (Vector3d.Magnitude(mScaledPosition) < 1000000000)
        {
            if (gameObject.name == "Sun")
            {
                transform.localScale = transform.localScale * planetSizeScale;
            }
            else
            {
                Transform child = transform.GetChild(0);
                Debug.Log($"orbiter: {child.gameObject.name}, {child.localScale}");
                child.localScale = child.localScale * planetSizeScale / SolarSystemManager.PlanetOnlyScale;
            }
        }
           

        // Debug.Log(gameObject.name + ": " + mPosition);
    }

    void FixedUpdate()
    {
        //Debug.Log(gameObject.name + ": " +  mScaledPosition.ToString() + " ; " + Vector3d.Magnitude(mScaledPosition));
        if (Vector3d.Magnitude(mScaledPosition) < 1000000000)
        {
            // Show planet in from of us
            // find some way to lock on & leave
    
            Vector3d gamePosition = mScaledPosition / closeToPlanetScale;

            // limit how close we get to planet
            float limit = 30;
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
