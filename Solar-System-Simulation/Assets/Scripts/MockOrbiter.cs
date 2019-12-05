using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockOrbiter : MonoBehaviour
{
    public GameObject ReferenceObject;
    void Start()
    {
        Transform SolarSystemTransform = GameObject.Find("SolarSystem").transform;
        string searchString = gameObject.name.Substring(4);

        if (searchString == "Sun")
        {
            ReferenceObject = SolarSystemTransform.Find(searchString).gameObject;
        } else
        {
            ReferenceObject = SolarSystemTransform.Find(searchString + "System").gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // other.gameObject.name == RightHandAnchor || LeftHandAnchor
        if (other.gameObject.name == "RightHandPointerCollider" || other.gameObject.name == "LeftHandPointerCollider")
        {
            Debug.Log($"here2: {other.gameObject.name}, {gameObject.name}");
            MockSolarSystemManager manager = transform.parent.GetComponent<MockSolarSystemManager>();
            manager.SelectedPlanets[gameObject.name] = !manager.SelectedPlanets[gameObject.name];
            manager.updateSelectedPlanets();
        }
     
    }
}
