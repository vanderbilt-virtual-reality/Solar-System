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

    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private MouseLook m_MouseLook;

    private Camera m_Camera;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private Vector3 m_OriginalCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        // TODO: remove
       // m_MouseLook.Init(transform , transform);
//        m_MouseLook.Init(transform , m_Camera.transform);
        mPosition = new Vector3d(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        RotateView();
    }

    void FixedUpdate()
    {
        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = m_Camera.transform.forward*m_Input.y + m_Camera.transform.right*m_Input.x;

        speed *= FindObjectOfType<SolarSystemManager>().TimeScale;

        m_MoveDir.x = desiredMove.x*speed;
        m_MoveDir.z = desiredMove.z*speed;
        m_MoveDir.y = desiredMove.y*speed;

        mPosition = mPosition + new Vector3d(m_MoveDir) * Time.fixedDeltaTime;
        //transform.position = transform.position + m_MoveDir*Time.fixedDeltaTime;

        Debug.Log(mPosition);

        //UpdateCameraPosition(speed);
        //m_MouseLook.UpdateCursorLock();
    }


    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");


// #if !MOBILE_INPUT
//             // On standalone builds, walk/run speed is modified by a key press.
//             // keep track of whether or not the character is walking or running
//             m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
// #endif
        // set the desired speed to be walking or running
        speed = m_MoveSpeed;
        m_Input = new Vector2(horizontal, vertical);

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



        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
        {
            transform.Rotate(0.0f, -0.3f, 0.0f, Space.World);
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))
        {
            transform.Rotate(0.0f, 0.3f, 0.0f, Space.World);
        }
    }
}
