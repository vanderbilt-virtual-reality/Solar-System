using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATSun : MonoBehaviour { 
    public AudioClip sunInfo;
    AudioSource audioSource; 


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        audioSource.PlayOneShot(sunInfo, 0.7f);
    }
    private void OnTriggerExit(Collider other)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
    }
}
