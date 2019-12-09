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
    private CharacterMovement CharacterMovement;
    private float startShowingPlanet = 10000000000;
    AudioSource m_MyAudioSource;
    private ConsoleScreenFront consoleScreenFront;

    private Dictionary<string, Dictionary<string, string>> planetsDetails = new Dictionary<string, Dictionary<string, string>>
    {
        {"Mercury",
            new Dictionary<string, string>{
                { "distance", "5.98e7 km" },
                {"radius", "2440 km" },
                {"type", "Terrestrial" },
                {"accessories", "No moons" },
            }
        },
        {"Venus",
            new Dictionary<string, string>{
                { "distance", "1.05e8 km" },
                {"radius", "6052 km" },
                {"type", "Terrestrial" },
                {"accessories", "No moons" },
            }
        },
        {"Earth",
            new Dictionary<string, string>{
                { "distance", "1.49e8 km" },
                {"radius", "6371 km" },
                {"type", "Terrestrial" },
                {"accessories", "1 moon" },
            }
        },
        {"Mars",
            new Dictionary<string, string>{
                { "distance", "2.24e8 km" },
                {"radius", "3390 km" },
                {"type", "Terrestrial" },
                {"accessories", "2 moons" },
            }
        },
        {"Jupiter",
            new Dictionary<string, string>{
                { "distance", "7.78e8 km" },
                {"radius", "69,911 km" },
                {"type", "Gas Giant" },
                {"accessories", "79 moons and has rings" },
            }
        },
        {"Saturn",
            new Dictionary<string, string>{
                { "distance", "1.42e9 km" },
                {"radius", "58,232 km" },
                {"type", "Gas Giant" },
                {"accessories", "89 moons and has rings" },
            }
        },
        {"Uranus",
            new Dictionary<string, string>{
                { "distance", "2.96e9 km" },
                {"radius", "25,362 km" },
                {"type", "Ice Giant" },
                {"accessories", "27 moons and has rings" },
            }
        },
        {"Neptune",
            new Dictionary<string, string>{
                { "distance", "4.49e9 km" },
                {"radius", "24,622 km" },
                {"type", "Ice Giant" },
                {"accessories", "14 moons and has rings" },
            }
        },
        {"Pluto",
            new Dictionary<string, string>{
                { "distance", "5.83e9 km" },
                {"radius", "1151 km" },
                {"type", "Not a planet" },
                {"accessories", "RIP Pluto 2006" },
            }
        },
    };

    // Start is called before the first frame update
    void Start()
    {
        SolarSystemManager = GameObject.FindObjectOfType<SolarSystemManager>();
        CharacterMovement = GameObject.FindObjectOfType<CharacterMovement>();
        consoleScreenFront = GameObject.FindObjectOfType<ConsoleScreenFront>(); 
        mPosition = new Vector3d(X, Y, Z);
        planetSizeScale = startShowingPlanet / closeToPlanetScale;


        if (gameObject.name == "Sun")
        {
            m_MyAudioSource = GetComponent<AudioSource>();

        }
        else
        {
            m_MyAudioSource = transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        planetSizeScale = startShowingPlanet / closeToPlanetScale;

        if (Vector3d.Magnitude(mScaledPosition) < startShowingPlanet)
        {
            double ratio = Vector3d.Magnitude(mScaledPosition) / startShowingPlanet;

            //CharacterMovement.m_MoveSpeed *= (float) ratio;

            float sizeIncrease = (float) Mathd.Lerp(0, 2.5, 1-ratio);
            m_MyAudioSource.mute = false;
            

            if (gameObject.name == "Sun")
            {

                transform.localScale = transform.localScale * planetSizeScale * sizeIncrease * 10000000;
                m_MyAudioSource.maxDistance = Vector3.Magnitude(transform.localScale) * 4;
            }
            else
            {
                Transform child = transform.GetChild(0);
                Debug.Log($"Scale: {child.localScale}");
                Debug.Log($"Audio Clip: {m_MyAudioSource.clip.ToString()}");
                child.localScale = child.localScale * planetSizeScale / SolarSystemManager.PlanetOnlyScale * sizeIncrease;



                // change the scale of the audio source
                m_MyAudioSource.maxDistance = Vector3.Magnitude(transform.GetChild(0).localScale) * 4;

            }
        } 
        else
        {
            m_MyAudioSource.mute = true;
        }
           

        // Debug.Log(gameObject.name + ": " + mPosition);
    }

    void FixedUpdate()
    {
        //Debug.Log(gameObject.name + ": " +  mScaledPosition.ToString() + " ; " + Vector3d.Magnitude(mScaledPosition));
        if (Vector3d.Magnitude(mScaledPosition) < startShowingPlanet)
        {
            consoleScreenFront.showWarningText(true, gameObject.name);

            string name = gameObject.name;

            if (name != "Sun")
            {
                name = name.Remove(name.Length - 6, 6);
                consoleScreenFront.showDetails(true, planetsDetails[name], gameObject.name);
            }
      

            // Show planet in front of us
            // find some way to lock on & leave
    
            Vector3d gamePosition = mScaledPosition / closeToPlanetScale;

            // limit how close we get to planet
            //Debug.Log($"gamePosition: {gamePosition}");
            //float limit = 2500;
            //if (Vector3d.Magnitude(gamePosition) < limit)
            //{
            //    gamePosition = gamePosition * limit / Vector3d.Magnitude(gamePosition); // rescale to 10m away
            //}
            float limit;
            if (gameObject.name == "Sun")
            {
                limit = Vector3.Magnitude(transform.localScale) * 2;
            }
            else
            {
                limit = Vector3.Magnitude(transform.GetChild(0).localScale) * 2;
            }
            if (Vector3d.Magnitude(gamePosition) < limit)
            {
                gamePosition = gamePosition * limit / Vector3d.Magnitude(gamePosition); // rescale to 10m away
            }

            if (!double.IsNaN(gamePosition.x))
            {
                transform.position = new Vector3((float)gamePosition.x, (float)gamePosition.y, (float)gamePosition.z);
            }

        }
        else
        {
            transform.position = new Vector3(0f, -100f, 0);
            consoleScreenFront.showWarningText(false, gameObject.name);
            consoleScreenFront.showDetails(false, null, gameObject.name);
        }
    }
}
