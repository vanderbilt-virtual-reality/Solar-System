using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial_Script : MonoBehaviour
{
    public GameObject c;
    public GameObject character;
    private float startAngle;
    private CharacterMovement CharacterMovement;
    // Start is called before the first frame update

    void Start()
    {
        startAngle = c.transform.localRotation.y;   
    }

    // Update is called once per frame
    void Update()
    {
     float currentAngle = c.transform.localRotation.y;

     float oldSpeed = character.GetComponent<CharacterMovement>().m_MoveSpeed;

        float speed = oldSpeed * (currentAngle-startAngle);
    }
}
