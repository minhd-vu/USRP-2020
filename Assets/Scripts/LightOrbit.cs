using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrbit : MonoBehaviour
{
    public Transform planet = null;
    public float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(planet);
        transform.RotateAround(planet.position, planet.up, speed * Time.deltaTime);
    }
}
