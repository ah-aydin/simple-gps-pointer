using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField] class GPSLocation : MonoBehaviour
{
    [SerializeField] Text GPSStatus;
    [SerializeField] Text latitudeValue;
    [SerializeField] Text longitudeValue;
    [SerializeField] Text altitudeValue;
    [SerializeField] Text horizontalAccuracyValue;
    [SerializeField] Text timeStampValue;

    [HideInInspector] float latitude;
    [HideInInspector] float longitude;
    [HideInInspector] float altitude;
    [HideInInspector] float horizontalAccuracy;
    [HideInInspector] double timeStamp;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GPSLoc());
    }

    IEnumerator GPSLoc()
    {
        // Check if the user has the location service enabled on their mobile phone
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service
        Input.location.Start();

        // Wait until service initialize
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            --maxWait;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            GPSStatus.text = "Time out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            GPSStatus.text = "Running";
            InvokeRepeating("UpdateGPSData", 0.5f, 1.0f);
        }
    }

    private void UpdateGPSData()
    {
        if (Input.location.status != LocationServiceStatus.Running)
        {
            GPSStatus.text = "Stop";
            return;
        }

        // Service is running
        if (GPSStatus != null) GPSStatus.text = "Running";
        
        latitude = Input.location.lastData.latitude;
        if (latitudeValue != null) latitudeValue.text  = latitude.ToString();

        longitude = Input.location.lastData.longitude;
        if (longitudeValue != null) longitudeValue.text = longitude.ToString();

        altitude = Input.location.lastData.altitude;
        if (altitudeValue != null) altitudeValue.text = altitude.ToString();

        horizontalAccuracy = Input.location.lastData.horizontalAccuracy;
        if (horizontalAccuracyValue != null) horizontalAccuracyValue.text = horizontalAccuracy.ToString();

        timeStamp = Input.location.lastData.timestamp;
        if (timeStampValue != null) timeStampValue.text = timeStamp.ToString();
    }
}
