using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class StarCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = -UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.CenterEye);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = -UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.CenterEye);
    }
}
