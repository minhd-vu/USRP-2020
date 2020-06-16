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
    public float m_MaxOrthographicSize;

    private float m_TargetOrthographicSize;
    private float m_VelocityOrthographicSize;
    private Camera cam;

    private Vector3 m_OrthographicPosition;
    private Vector3 m_PerspectivePosition;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        m_OrthographicPosition = new Vector3(0, 100, -100);
        m_PerspectivePosition = new Vector3(0, 5, 0);
        m_TargetOrthographicSize = cam.orthographicSize;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
    }
    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cam.orthographic = !cam.orthographic;
            }

            Vector3 position = cam.orthographic ? m_OrthographicPosition : m_PerspectivePosition;

            if (Input.GetKey(KeyCode.W))
            {
                position.z += Time.deltaTime * (cam.orthographic ? cam.orthographicSize : m_CameraSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                position.x -=Time.deltaTime * (cam.orthographic ? cam.orthographicSize : m_CameraSpeed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                position.z -= Time.deltaTime * (cam.orthographic ? cam.orthographicSize : m_CameraSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                position.x += Time.deltaTime * (cam.orthographic ? cam.orthographicSize : m_CameraSpeed);
            }

            if (!cam.orthographic)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    position.y += m_CameraSpeed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    position.y -= m_CameraSpeed * Time.deltaTime;
                }

                m_PerspectivePosition = position;
            }
            else
            {
                m_OrthographicPosition = position;
            }

            transform.position = position;

            float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (mouseWheelInput != 0.0f && cam.orthographic)
            {
                m_TargetOrthographicSize -= mouseWheelInput * m_ZoomSensitivity;
                m_TargetOrthographicSize = Mathf.Clamp(m_TargetOrthographicSize, m_MinOrthographicSize, m_MaxOrthographicSize);
            }
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
