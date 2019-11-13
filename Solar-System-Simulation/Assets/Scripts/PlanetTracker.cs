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
        public GameObject gameObject;
        public double distance;
    }

    public struct HitObj {
        public RaycastHit hit;
        public string name;
        public double distance;
    }

    public String[] m_NamesToTrack;
    public bool m_ShowByName = false; // TODO: implement this
   // [SerializeField] private int m_IndexToShow = 0; // show the first closest planet / obj


    // Start is called before the first frame update
    void Start()
    {
        drawOnWindshield = GameObject.Find("Windshield").transform.Find("Canvas").GetComponent<DrawOnWindshield>();
        GameObject[] gmArr = GameObject.FindGameObjectsWithTag("Planet");

        List<Obj> objsList = new List<Obj>();

        foreach (GameObject gm in gmArr) {
            objsList.Add(new Obj {gameObject=gm, distance=0});
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
            trackPlanets(m_objects.Where(obj => m_NamesToTrack.Contains(obj.gameObject.name)).ToArray());
        }
        else
        {
            // show the number of closest ones
            trackPlanets(m_objects.OrderByDescending(obj => obj.distance).Reverse().Take(m_NumberToShow).ToArray());
        }
    }

    private void updateDistances() {
        // update distance
        for (int i = 0; i < m_objects.Length; ++i) {
            m_objects[i].distance = Vector3.Magnitude(transform.position - m_objects[i].gameObject.transform.position);
        }
    }

    void trackPlanets(Obj[] planets) {
        List<HitObj> hitObjs = new List<HitObj>();

        foreach(Obj o in planets)
        {

            RaycastHit hit;
            Vector3 direction = Vector3.Normalize(o.gameObject.transform.position - transform.position);

            // if (Physics.Raycast(transform.position, direction, out hit, (int)m_MaxDistance))
            if (Physics.Raycast(transform.position, direction, out hit))
            {
                if (hit.collider.tag == "PlanetIntersector")
                {
                    Debug.DrawRay(transform.position, direction * 10);

                    // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    // sphere.transform.position = hit.point;
                    // sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    hitObjs.Add(new HitObj {hit=hit, name=o.gameObject.name, distance=o.distance});
                }

            }
        }

        drawOnWindshield.drawPointsWithName(hitObjs);
    }
}
