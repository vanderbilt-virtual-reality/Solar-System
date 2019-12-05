using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

public class CharacterMovement : MonoBehaviour
{
    //[Serializable]
    //public class MouseLook
    //{
    //    public float XSensitivity = 2f;
    //    public float YSensitivity = 2f;
    //    public bool clampVerticalRotation = true;
    //    public float MinimumX = -90F;
    //    public float MaximumX = 90F;
    //    public bool smooth;
    //    public float smoothTime = 5f;
    //    public bool lockCursor = true;


    //    private Quaternion m_CharacterTargetRot;
    //    private Quaternion m_CameraTargetRot;
    //    private bool m_cursorIsLocked = true;

    //    public void Init(Transform character, Transform camera)
    //    {
    //        m_CharacterTargetRot = character.localRotation;
    //        m_CameraTargetRot = camera.localRotation;
    //    }


    //    public void LookRotation(Transform character, Transform camera)
    //    {
    //        float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
    //        float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

    //        // TODO: remove
    //        m_CharacterTargetRot *= Quaternion.Euler (-xRot, yRot, 0f);
    //        m_CameraTargetRot *= Quaternion.Euler (-xRot, yRot, 0f);
    //        //m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
    //        //m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

    //        if(clampVerticalRotation)
    //            m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

    //        if(smooth)
    //        {
    //            character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
    //                smoothTime * Time.deltaTime);
    //            camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
    //                smoothTime * Time.deltaTime);
    //        }
    //        else
    //        {
    //            character.localRotation = m_CharacterTargetRot;
    //            camera.localRotation = m_CameraTargetRot;
    //        }

    //        UpdateCursorLock();
    //    }

    //    public void SetCursorLock(bool value)
    //    {
    //        lockCursor = value;
    //        if(!lockCursor)
    //        {//we force unlock the cursor if the user disable the cursor locking helper
    //            Cursor.lockState = CursorLockMode.None;
    //            Cursor.visible = true;
    //        }
    //    }

    //    public void UpdateCursorLock()
    //    {
    //        //if the user set "lockCursor" we check & properly lock the cursos
    //        if (lockCursor)
    //            InternalLockUpdate();
    //    }

    //    private void InternalLockUpdate()
    //    {
    //        if(Input.GetKeyUp(KeyCode.Escape))
    //        {
    //            m_cursorIsLocked = false;
    //        }
    //        else if(Input.GetMouseButtonUp(0))
    //        {
    //            m_cursorIsLocked = true;
    //        }

    //        if (m_cursorIsLocked)
    //        {
    //            Cursor.lockState = CursorLockMode.Locked;
    //            Cursor.visible = false;
    //        }
    //        else if (!m_cursorIsLocked)
    //        {
    //            Cursor.lockState = CursorLockMode.None;
    //            Cursor.visible = true;
    //        }
    //    }

    //    Quaternion ClampRotationAroundXAxis(Quaternion q)
    //    {
    //        q.x /= q.w;
    //        q.y /= q.w;
    //        q.z /= q.w;
    //        q.w = 1.0f;

    //        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

    //        angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

    //        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

    //        return q;
    //    }

    //}

    public Vector3d mPosition;
    public Vector3d mScaledPosition;

    [SerializeField] public float m_MoveSpeed;
    [SerializeField] private GameObject m_StarCameraController;
    [SerializeField] private float m_SpeedScale;
    [SerializeField] private Vector3 m_StartPosition;

    private Camera m_Camera;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private Vector3 m_OriginalCameraPosition;
    private float holdButtonScale = 1;
   

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        // TODO: remove
       // m_MouseLook.Init(transform , transform);
//        m_MouseLook.Init(transform , m_Camera.transform);
        mPosition = new Vector3d(m_StartPosition);
    }

    // Update is called once per frame
    void Update()
    {
        RotateView();
        UpdateSpeed();
    }

    void FixedUpdate()
    {
        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y; //+ transform.right*m_Input.x;

        speed *= FindObjectOfType<SolarSystemManager>().TimeScale;

        m_MoveDir.x = desiredMove.x*speed;
        m_MoveDir.z = desiredMove.z*speed;
        m_MoveDir.y = 0;

        mPosition = mPosition + new Vector3d(m_MoveDir) * Time.fixedDeltaTime;
        //transform.position = transform.position + m_MoveDir*Time.fixedDeltaTime;


        //UpdateCameraPosition(speed);
        //m_MouseLook.UpdateCursorLock();
    }


    private void GetInput(out float speed)
    {
        // Read input
        m_Input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);


// #if !MOBILE_INPUT
//             // On standalone builds, walk/run speed is modified by a key press.
//             // keep track of whether or not the character is walking or running
//             m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
// #endif
        // set the desired speed to be walking or running
        speed = m_MoveSpeed;

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }
    }

    private void RotateView()
    {
        // TODO: remove
        //m_MouseLook.LookRotation (transform, transform);
        // m_MouseLook.LookRotation (transform, m_Camera.transform);



        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft))
        {
            transform.Rotate(0.0f, -0.3f, 0.0f, Space.World);
            m_StarCameraController.transform.Rotate(0.0f, -0.3f, 0.0f, Space.World);
            
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
        {
            transform.Rotate(0.0f, 0.3f, 0.0f, Space.World);
            m_StarCameraController.transform.Rotate(0.0f, 0.3f, 0.0f, Space.World);
        }
    }

    private void UpdateSpeed()
    {
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        // The longer you hold the speed button the faster it scales
        if (input.y != 0)
        {
            holdButtonScale += 0.01f;
        }
        else
        {
            holdButtonScale = 1;
        }

        m_MoveSpeed += input.y * m_SpeedScale * holdButtonScale;
        if (m_MoveSpeed <= 1)
        {
            m_MoveSpeed = 1;
        }
    }
}
