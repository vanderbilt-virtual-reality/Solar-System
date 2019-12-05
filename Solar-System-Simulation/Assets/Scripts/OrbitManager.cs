using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitManager : MonoBehaviour
{
    private SolarSystemManager MySolarSystemManager;
    // Start is called before the first frame update
    void Start()
    {
        MySolarSystemManager = FindObjectOfType<SolarSystemManager>();
    }

    void FixedUpdate()
    {
        // TODO: elliptical orbits
        foreach(Transform child in transform) {
            Orbiter o = child.gameObject.GetComponent<Orbiter>();
            if (o != null) {
                Vector3d pivot = new Vector3d(transform.position);
                Vector3d newVec = o.mPosition;
                double r = Mathd.Sqrt(newVec.x * newVec.x + newVec.z * newVec.z);
                double theta = Mathd.Atan2(newVec.z, newVec.x);
                theta +=  MySolarSystemManager.TimeScale * Time.deltaTime * 360 / o.OrbitTime * Mathd.PI / 180;
                newVec.x = r * Mathd.Cos(theta);
                newVec.z = r * Mathd.Sin(theta);

                o.mPosition = newVec;

                // double increment = MySolarSystemManager.TimeScale * Time.deltaTime * 360 / o.OrbitTime;
                // o.mPosition = RotateAroundPoint(o.mPosition, new Vector3d(transform.position),new Vector3d(o.OrbitAxis) * increment);
                //  (o.mPosition - pivot)
                //     * (new Vector3d(o.OrbitAxis) * increment)
                //     + pivot;


                //          alpha += 10 ;
                //  ;
                //  X = x + (a * Mathf.Cos(alpha*.005));
                //  Y= y + (b * Mathf.Sin(alpha*.005));
                //  this.gameObject.transform.position = Vector3(X,0,Y);
                // o.transform.RotateAround(
                //     transform.position, 
                //     o.OrbitAxis, 
                //     (float) (MySolarSystemManager.TimeScale * Time.deltaTime * 360 / o.OrbitTime)
                // );
            }

        }
    }
}
