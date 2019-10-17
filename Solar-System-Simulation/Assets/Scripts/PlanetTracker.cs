using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanetTracker : MonoBehaviour
{
    private DrawOnWindshield drawOnWindshield;

    private struct Obj {
        public GameObject gameObject;
        public double distance;
    }
    // TODO:
    // Get list of planet objects
    private Obj[] m_objects = {};

    [SerializeField] private double m_MinDistance = 0d;
    [SerializeField] private double m_MaxDistance = 1000d;
    [SerializeField] private int m_NumberToShow = 3;
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

        updateList();
        
        // TODO: sort objects based on distance from user
        Array.Sort(m_objects, (x, y) => {
            if (x.distance < y.distance) {
                return -1;
            } else if (x.distance > y.distance) {
                return 1;
            } else {
                return 1;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        updateList();

        List<RaycastHit> hits = new List<RaycastHit>();
        List<string> names = new List<string>();

        int numberShown = 0; // TODO

        foreach(Obj o in m_objects) {
            if (numberShown == m_NumberToShow) break;

            RaycastHit hit;
            Vector3 direction = Vector3.Normalize(o.gameObject.transform.position - transform.position);
            float distance = 100; // TODO: this should be limited even more

            if (Physics.Raycast(transform.position, direction, out hit, distance)) {
                if (hit.collider.tag == "PlanetIntersector" && numberShown++ < m_NumberToShow) {
                    //Debug.Log(hit.point);
                    Debug.DrawRay(transform.position, direction * 10);
                    // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    // sphere.transform.position = hit.point;
                    // sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    
                    hits.Add(hit);
                    names.Add(o.gameObject.name);
                    // TODO: draw on canvas at this location
                }
   
            }
        }

        drawOnWindshield.drawPointsWithName(hits, names);
    }

    void updateList() {
        for (int i = 0; i < m_objects.Length; ++i) {
            m_objects[i].distance = Vector3.Magnitude(transform.position - m_objects[i].gameObject.transform.position);
        }

        for (int i = 1; i < m_objects.Length; ++i) {
            if (m_objects[i].distance < m_objects[i-1].distance) {
                Obj temp = m_objects[i];
                m_objects[i] = m_objects[i-1];
                m_objects[i - 1] = temp;
            }
        }
    }
}
