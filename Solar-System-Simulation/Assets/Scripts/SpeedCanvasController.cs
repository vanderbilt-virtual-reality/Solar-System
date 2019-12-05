using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpeedCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject Character;
    private CharacterMovement m_CharacterMovement;
    private GameObject m_SpeedText;
    // Start is called before the first frame update
    void Start()
    {
        m_CharacterMovement = Character.GetComponent<CharacterMovement>();
        m_SpeedText = transform.Find("Speed").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        m_SpeedText.GetComponent<Text>().text = $"Speed:\n{Math.Round(m_CharacterMovement.m_MoveSpeed)}m";
    }
}
