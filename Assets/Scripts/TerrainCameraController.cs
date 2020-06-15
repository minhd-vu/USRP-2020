using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCameraController : MonoBehaviour
{
    public float m_CameraSpeed = 20.0f;
    public float m_CameraMotionBorder = 10.0f;
    public float m_ZoomSensitivity = 30.0f;
    public float m_ZoomTime = 0.2f;
    public float m_MinOrthographicSize = 5.0f;
    public float m_MaxOrthographicSize = 50.0f;

    private float m_TargetOrthographicSize;
    private float m_VelocityOrthographicSize;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        m_TargetOrthographicSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - m_CameraMotionBorder)
        {
            position.z += m_CameraSpeed * Time.deltaTime * cam.orthographicSize;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= m_CameraMotionBorder)
        {
            position.x -= m_CameraSpeed * Time.deltaTime * cam.orthographicSize;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= m_CameraMotionBorder)
        {
            position.z -= m_CameraSpeed * Time.deltaTime * cam.orthographicSize;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - m_CameraMotionBorder)
        {
            position.x += m_CameraSpeed * Time.deltaTime * cam.orthographicSize;
        }

        transform.position = position;

        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheelInput != 0.0f && cam.orthographic)
        {
            m_TargetOrthographicSize -= mouseWheelInput * m_ZoomSensitivity;
            m_TargetOrthographicSize = Mathf.Clamp(m_TargetOrthographicSize, m_MinOrthographicSize, m_MaxOrthographicSize);
        }
    }

    void LateUpdate()
    {
        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, m_TargetOrthographicSize, ref m_VelocityOrthographicSize, m_ZoomTime);
        }
    }
}
