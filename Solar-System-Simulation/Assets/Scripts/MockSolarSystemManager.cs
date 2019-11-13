using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System;
using System.Linq;

public class MockSolarSystemManager : MonoBehaviour
{
    private Transform Character;
    private PlanetTracker m_PlanetTracker;
    private OrderedDictionary scale_dict = new OrderedDictionary(){
            {"MockSun", 1},
            {"MockMercury", 3},
            {"MockVenus", 3},
            {"MockEarth", 3},
            {"MockMars", 3},
            {"MockJupiter", 1.15f}, 
            {"MockSaturn", .8f},
            {"MockUranus", 0.5f},
            {"MockNeptune", 0.38f},
            {"MockPluto", .336f}
        };

    private double[] planet_distances = {0, .005791, .01082, .01496, .02279, .07785, .1434, .2871, .4495, .5906};
    
    private double scaleFactor = 10000000000000;
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

    void FixedUpdate()
    {
        // MockOrbiter has reference to real object
        // map position of that object to local position and divide by a large amount

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

        Debug.Log($"Character pos:{Character.position}, shipPos:{shipPos}");
        
        ship.localPosition = new Vector3(shipPos.z, shipPos.x, shipPos.y);

        int index = 0;
        Transform sunTransform = transform.Find("MockSun");
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.ToLower() == "ship") continue;

            GameObject o = child.gameObject.GetComponent<MockOrbiter>().ReferenceObject;

            Vector3 newVec = o.transform.position * (float) Convert.ToDouble(scale_dict[child.gameObject.name]) / (10000000000000);


            child.localPosition = new Vector3(newVec.z, newVec.x, newVec.y);
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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SelectedPlanets["MockSun"] = !SelectedPlanets["MockSun"];
            updateSelectedPlanets();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedPlanets["MockMercury"] = !SelectedPlanets["MockMercury"];
            updateSelectedPlanets();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedPlanets["MockVenus"] = !SelectedPlanets["MockVenus"];
            updateSelectedPlanets();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("HERE");
            SelectedPlanets["MockEarth"] = !SelectedPlanets["MockEarth"];
            updateSelectedPlanets();
        }
    }

    // should be called only when a new planet is selected
    public void updateSelectedPlanets()
    {
        foreach(KeyValuePair<string, bool> selectedPlanet in SelectedPlanets)
        {
            Material mat = transform.Find(selectedPlanet.Key).gameObject.GetComponent<MeshRenderer>().material;
            bool enabled = mat.IsKeywordEnabled("_EMISSION");

            // Turn on
            if (selectedPlanet.Value && !enabled)
            {
                mat.EnableKeyword("_EMISSION");

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
