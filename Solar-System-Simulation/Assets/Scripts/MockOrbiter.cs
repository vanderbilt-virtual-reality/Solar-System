using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockOrbiter : MonoBehaviour
{

    private MockSolarSystemManager manager;
    public GameObject ReferenceObject;
    private float collisionDelay = 0.5f;
    private bool isColliding = false;
    void Start()
    {
        manager = transform.parent.GetComponent<MockSolarSystemManager>();
        Transform SolarSystemTransform = GameObject.Find("SolarSystem").transform;
        string searchString = gameObject.name.Substring(4);

        if (searchString == "Sun")
        {
            ReferenceObject = SolarSystemTransform.Find(searchString).gameObject;
        }
        else
        {
            ReferenceObject = SolarSystemTransform.Find(searchString + "System").gameObject;
        }
    }

    private void Update()
    {
        if (collisionDelay <= 0)
        {
            isColliding = false;
            collisionDelay = 0.5f;
        }
        if (isColliding)
        {
            collisionDelay -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;
        Debug.Log($"enter {gameObject.name}");
        // other.gameObject.name == RightHandAnchor || LeftHandAnchor
        if (other.gameObject.name == "RightHandPointerCollider" || other.gameObject.name == "LeftHandPointerCollider")
        {
            Debug.Log($"here2: {other.gameObject.name}, {gameObject.name}");
            manager.SelectedPlanets[gameObject.name] = !manager.SelectedPlanets[gameObject.name];
            manager.updateSelectedPlanets();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"exit {gameObject.name}");
    }
}
