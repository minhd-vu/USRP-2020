using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject primaryCamera;
    public GameObject secondaryCamera;

    void Start()
    {
        primaryCamera.SetActive(true);
        secondaryCamera.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            primaryCamera.SetActive(!primaryCamera.activeSelf);
            secondaryCamera.SetActive(!secondaryCamera.activeSelf);
        }
    }
}
