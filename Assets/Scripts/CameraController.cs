using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraController reads the player's input and allows them to click and drag to spin around the planet,
// and use the mouse wheel to zoom in and out.

public class CameraController : MonoBehaviour
{
    private Camera cam;

    public float m_MouseDragSensitivity = 1.0f; // How fast does the planet spin when you click and drag?
    public float m_MouseZoomSensitivity = 1.0f; // How fast does the mouse wheel zoom you in and out?

    public float minFOV = 30f;
    public float maxFOV = 90f;
    public float zoomSensitivity = 30f;
    public float zoomTime = 0.2f;

    private Vector3 m_PrevMousePosition; // In order to track how the player is dragging the mouse, we need to store what
                                         // the previous mouse position was from the last frame.

    public float m_Zoom;              // How zoomed-in the camera is.

    private float velocityFOV;
    private float targetFOV;

    private void Start()
    {
        // Set the camera object.
        cam = gameObject.GetComponent<Camera>();

        // Set the intial camera fov.
        targetFOV = cam.fieldOfView;
    }

    void LateUpdate()
    {
        // Update the field of view.
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetFOV, ref velocityFOV, zoomTime);
    }

    void Update()
    {
        Vector3 currentMousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            // When the user first clicks the mouse button, we snap m_PrevMousePosition to the current mouse position.
            // This solves a problem where, if the application doesn't have focus, m_PrevMousePosition won't be updated.
            // And when the player clicks inside the window give the application focus, there could be an arbitrarily
            // large difference between m_PrevMousePosition and currentMousePosition;

            m_PrevMousePosition = currentMousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            // So long as the mouse button is held down, track how far it has been moved in screen-space each frame.

            Vector3 mouseDisplacement = currentMousePosition - m_PrevMousePosition;

            mouseDisplacement.x /= Screen.width;
            mouseDisplacement.y /= Screen.height;

            // Moving the mouse left and right results in a rotation around the camera's own up axis.

            Quaternion yaw = Quaternion.AngleAxis(mouseDisplacement.x * m_MouseDragSensitivity, transform.up);

            // And moving the mouse up and down results in a rotation around the camera's right axis.

            Quaternion pitch = Quaternion.AngleAxis(mouseDisplacement.y * m_MouseDragSensitivity, -transform.right);

            // Since we have world-space rotations that we want to apply to the camera transform, we need to
            // multiply from the left. As in "yaw * transform.localRotation" rather than "transform.localRotation * yaw".

            transform.localRotation = yaw * pitch * transform.localRotation;
        }

        // Read the mouse wheel input, and move our fov parameter between the minimum and maximum allowed values.

        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheelInput != 0.0f)
        {
            targetFOV -= mouseWheelInput * zoomSensitivity;
            targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
        }

        // One final step. We always want the camera to face the planet, and the simplest way to ensure that
        // is to back the camera up along its own forward axis, until it's at the correct distance set in m_Zoom.

        transform.position = transform.forward * -m_Zoom;

        // Okay one final, FINAL step. We need to save the current mouse position to m_PrevMousePosition, so
        // that we can keep tracking the mouse's movement next frame.

        m_PrevMousePosition = currentMousePosition;
    }
}
