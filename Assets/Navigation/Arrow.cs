using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject camera;

    GPSLocation gpsLocation;

    // Start is called before the first frame update
    void Start()
    {
        gpsLocation = FindObjectOfType<GPSLocation>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
