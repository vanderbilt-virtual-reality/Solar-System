using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button_pressed : MonoBehaviour
{


  
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        transform.position = transform.position - new Vector3(0, (float)0, 0);


    }

}
