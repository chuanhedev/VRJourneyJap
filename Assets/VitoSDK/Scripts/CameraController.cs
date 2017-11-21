using UnityEngine;
using System.Collections;
using UnityEngine.VR;
public class CameraController : MonoBehaviour {
    void Start () {
        if(UnityEngine.VR.VRDevice.isPresent)
        {
            Destroy(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android)
            return;
        //if (UICamera.isOverUI)
        //    return;

        if(UnityEngine.EventSystems.EventSystem.current!=null&&
            (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()||UnityEngine.EventSystems.EventSystem.current.alreadySelecting))
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(transform.localEulerAngles.x >= 270 && transform.localEulerAngles.x <=360)
                m_LookAngle_x = transform.localEulerAngles.x-360;
            else
                m_LookAngle_x = transform.localEulerAngles.x;

            m_LookAngle_y = transform.localEulerAngles.y;
        }
        if (Input.GetMouseButton(0))
        {
            HandleRotationMovement();
        }
        
    }
    [Range(0f, 10f)]
    [SerializeField]
    private float m_TurnSpeed = 1.5f;   // How fast the rig will rotate from user input.
    [SerializeField]
    private float m_TurnSmoothing = 10.0f;                // How much smoothing to apply to the turn input, to reduce mouse-turn jerkiness

    private float m_TiltMin = 90;
    private float m_TiltMax = 90;

    private float m_LookAngle_y;                    // The rig's y axis rotation.
    private float m_LookAngle_x;                    // The rig's x axis rotation.
    private Quaternion m_TransformTargetRot;

    void Awake()
    {
        m_TransformTargetRot = transform.localRotation;
    }
    
    private void HandleRotationMovement()
    {
        // Read the user input
        var x = -Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        // Adjust the look angle by an amount proportional to the turn speed and horizontal input.\
        m_LookAngle_x += y * m_TurnSpeed;
        m_LookAngle_y += x * m_TurnSpeed;

        //Debug.Log(transform.eulerAngles + " " + m_LookAngle_x);
        
        m_LookAngle_x = Mathf.Clamp(m_LookAngle_x, -m_TiltMin, m_TiltMax);
        // Rotate the rig (the root object) around Y axis only:
        m_TransformTargetRot = Quaternion.Euler(m_LookAngle_x, m_LookAngle_y, 0f);
        
        if (m_TurnSmoothing > 0)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.unscaledDeltaTime);
        }
        else
        {
            transform.localRotation = m_TransformTargetRot;
        }
    }
}
