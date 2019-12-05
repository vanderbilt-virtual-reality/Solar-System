using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlanetTracker : MonoBehaviour
{
    private DrawOnWindshield drawOnWindshield;

    private Obj[] m_objects = { };

    // [SerializeField] private double m_MinDistance = 0d;
    // [SerializeField] private double m_MaxDistance = 1000d;
    [SerializeField] private int m_NumberToShow = 3;

    private struct Obj {
        public string name;
        public Orbiter orbiter;
        public double distance;
    }

    public struct HitObj {
        public RaycastHit hit;
        public string name;
        public double distance;
    }

    public String[] m_NamesToTrack;
    public bool m_ShowByName = false;
   // [SerializeField] private int m_IndexToShow = 0; // show the first closest planet / obj


    // Start is called before the first frame update
    void Start()
    {
        drawOnWindshield = GameObject.Find("Windshield").transform.Find("Canvas").GetComponent<DrawOnWindshield>();
        GameObject[] gmArr = GameObject.FindGameObjectsWithTag("Planet");

        List<Obj> objsList = new List<Obj>();

        foreach (GameObject gm in gmArr) {
            Orbiter o = gm.transform.parent.gameObject.GetComponent<Orbiter>();
            if (o != null)
            {
                objsList.Add(new Obj {name=gm.name, orbiter=o, distance=0});
            }
        }

        m_objects = objsList.ToArray();

        updateDistances();
    }

    // Update is called once per frame
    void Update()
    {
        updateDistances();

        if (m_ShowByName)
        {
            Debug.Log("here");
            trackPlanets(m_objects.Where(obj => m_NamesToTrack.Contains(obj.name)).ToArray());
        }
        else
        {
            Debug.Log($"m_NumberToShow: {m_NumberToShow}");
            // show the number of closest ones
            trackPlanets(m_objects.OrderByDescending(obj => obj.distance).Reverse().Take(m_NumberToShow).ToArray());
        }
    }

    private void updateDistances() {
        // update distance
        for (int i = 0; i < m_objects.Length; ++i) {
            m_objects[i].distance = Vector3d.Magnitude(/*new Vector3d(transform.position) - */m_objects[i].orbiter.mScaledPosition);
        }
    }

    void trackPlanets(Obj[] planets) {
        List<HitObj> hitObjs = new List<HitObj>();

        Debug.Log($"planets: {planets.Length}");

        foreach(Obj o in planets)
        {
            //Debug.Log(o.orbiter.gameObject.name);
            RaycastHit hit;
            Vector3d d = Vector3d.Normalize(o.orbiter.mScaledPosition - new Vector3d(transform.position));
            Vector3 direction = new Vector3((float) d.x, (float) d.y, (float) d.z);

            // if (Physics.Raycast(transform.position, direction, out hit, (int)m_MaxDistance))
            if (Physics.Raycast(transform.position, direction, out hit))
            {


                if (hit.collider.tag == "PlanetIntersector")
                {
                    Debug.DrawRay(transform.position, direction * 10);

                    // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    // sphere.transform.position = hit.point;
                    // sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    hitObjs.Add(new HitObj {hit=hit, name=o.name, distance=o.distance});
                }

            }
        }

        drawOnWindshield.drawPointsWithName(hitObjs);
    }
}
