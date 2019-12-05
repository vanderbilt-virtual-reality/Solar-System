using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialScript : MonoBehaviour
{
    private Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        //float y = transform.localRotation.y;
        //rotation.y = y;
        //transform.localRotation = rotation;
    }
}
