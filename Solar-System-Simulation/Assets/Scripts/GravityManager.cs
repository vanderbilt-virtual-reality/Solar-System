using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    [System.Serializable]
    public class GravityPair {
        public GameObject Object1;
        public GameObject Object2;
    }

    public GravityPair[] GravityPairs;

    public float scale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(GravityPair gp in GravityPairs)
        {
            Vector3 r = gp.Object1.transform.position - gp.Object2.transform.position;
            float totalForce = - (scale * gp.Object1.GetComponent<GravitationalBody>().mass * gp.Object2.GetComponent<GravitationalBody>().mass) / Vector3.Magnitude(r);
            Debug.Log(gp.Object2.transform.position.normalized);
            Debug.Log(gp.Object2.transform.position.normalized * -totalForce);
            gp.Object1.GetComponent<Rigidbody>().AddForce(gp.Object1.transform.position.normalized * totalForce);
            gp.Object2.GetComponent<Rigidbody>().AddForce(gp.Object2.transform.position.normalized * totalForce);
        }
    }
}
