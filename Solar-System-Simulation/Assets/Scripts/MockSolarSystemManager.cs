using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System;
using System.Linq;


/**
 * This class manages the mock solar system that exists on the ship
 */
public class MockSolarSystemManager : MonoBehaviour
{
    private Transform Character;
    private PlanetTracker m_PlanetTracker;
    private OrderedDictionary scale_dict = new OrderedDictionary(){
            {"MockSun", .01f},
            {"MockMercury", .03f},
            {"MockVenus", .03f},
            {"MockEarth", .03f},
            {"MockMars", .03f},
            {"MockJupiter", .0115f}, 
            {"MockSaturn", .008f},
            {"MockUranus", 0.005f},
            {"MockNeptune", 0.0038f},
            {"MockPluto", .00336f}
        };

    private double[] planet_distances = {0, .005791, .01082, .01496, .02279, .07785, .1434, .2871, .4495, .5906};
    
    [SerializeField] private double scaleFactor = 100000000000000;
    public Dictionary<string, bool> SelectedPlanets = new Dictionary<string, bool>();
    // TODO: (kinda done--need to fix the direction and test this when scaling is fixed)
    // In order to map the current position of the ship to the minimap correctly
    // use the value for the next planet as the correct positioning
    void Start()
    {
        Character = Camera.main.transform.parent;
        planet_distances = planet_distances.Select(d => d*scaleFactor).ToArray();
        foreach(DictionaryEntry obj in scale_dict)
        {
            SelectedPlanets.Add(Convert.ToString(obj.Key), false);
        }

        m_PlanetTracker = Camera.main.GetComponent<PlanetTracker>();
    }



    // FixedUpdate() is called every fixed frame-rate frame
    void FixedUpdate()
    {
        // MockOrbiter has reference to real object
        // map position of that object to local position and divide by a large amount

        // find the ship
        Transform ship = transform.Find("Ship");


        Vector3 shipPos = new Vector3();

        float[] scaleDictValues = new float[scale_dict.Values.Count];

        scale_dict.Values.CopyTo(scaleDictValues, 0);

        for (var i = 0; i < planet_distances.Length; ++i)
        {
            if (ship.position.magnitude <= planet_distances[i])
            {
                shipPos = Character.position * scaleDictValues[i] / (float) scaleFactor;
                break;
            }
        }

        // Debug.Log($"Character pos:{Character.position}, shipPos:{shipPos}");
        
        ship.localPosition = new Vector3(shipPos.z, shipPos.x, shipPos.y);

        int index = 0;
        Transform sunTransform = transform.Find("MockSun");

        // loops through each mock planet
        // update its local position based 
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.ToLower() == "ship") continue; //skip everything if it's a ship

            GameObject o = child.gameObject.GetComponent<MockOrbiter>().ReferenceObject;

            Orbiter orbiter = o.GetComponent<Orbiter>();

            if (orbiter != null)
            {
                Vector3d newVec = orbiter.mPosition * Convert.ToDouble(scale_dict[child.gameObject.name]) / (scaleFactor);
                child.localPosition = new Vector3((float) newVec.z, (float) newVec.x, (float) newVec.y);
            }

            // Vector3 newVec = o.transform.position * (float) Convert.ToDouble(scale_dict[child.gameObject.name]) / (10000000000000);

            // child.localPosition = new Vector3(newVec.z, newVec.x, newVec.y);
            



            //Vector3 orbitAxis = transform.TransformDirection(sunTransform.localRotation * Vector3.forward);

            // Debug.Log(MainCamera.transform.localRotation);
            // Debug.Log(MainCamera.transform.position);

            // Draws the axis to orbit around
            // Debug.DrawRay(sunTransform.position, orbitAxis, Color.yellow, 0.1f);
            ++index;
        }
    }

    void Update()
    {

        // TODO: REMOVE
        //if (Input.GetKeyDown("0"))
        //{
        //    SelectedPlanets["MockSun"] = !SelectedPlanets["MockSun"];
        //    updateSelectedPlanets();
        //}
        //if (Input.GetKeyDown("1"))
        //{
        //    SelectedPlanets["MockMercury"] = !SelectedPlanets["MockMercury"];
        //    updateSelectedPlanets();
        //}
        //if (Input.GetKeyDown("2"))
        //{
        //    SelectedPlanets["MockVenus"] = !SelectedPlanets["MockVenus"];
        //    updateSelectedPlanets();
        //}
        //if (Input.GetKeyDown("3"))
        //{
        //    SelectedPlanets["MockEarth"] = !SelectedPlanets["MockEarth"];
        //    updateSelectedPlanets();
        //}
    }

    

    // should be called only when a new planet is selected
    public void updateSelectedPlanets()
    {
        foreach(KeyValuePair<string, bool> selectedPlanet in SelectedPlanets)
        {
            MeshRenderer meshRenderer = transform.Find(selectedPlanet.Key).gameObject.GetComponent<MeshRenderer>();
            Material mat = meshRenderer.material;
            bool enabled = mat.IsKeywordEnabled("_EMISSION");

            // Turn on
            if (selectedPlanet.Value && !enabled)
            {
                mat.EnableKeyword("_EMISSION");
                meshRenderer.enabled = true;


                List<string> temp = m_PlanetTracker.m_NamesToTrack.ToList();
                if (!temp.Contains(selectedPlanet.Key.Substring(4)))
                {
                    temp.Add(selectedPlanet.Key.Substring(4));
                }
                m_PlanetTracker.m_NamesToTrack = temp.ToArray();
            }
            // Turn off
            else if (!selectedPlanet.Value && enabled)
            {
                mat.DisableKeyword("_EMISSION");
                meshRenderer.enabled = false;

                List<string> temp = m_PlanetTracker.m_NamesToTrack.ToList();
                if (temp.Contains(selectedPlanet.Key.Substring(4)))
                {
                    temp.Remove(selectedPlanet.Key.Substring(4));
                }
                m_PlanetTracker.m_NamesToTrack = temp.ToArray();            
            }
        }
    }
}
