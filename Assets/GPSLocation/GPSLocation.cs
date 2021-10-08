using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

[SerializeField] class GPSLocation : MonoBehaviour
{
    [SerializeField] GameObject camera;

    [SerializeField] Text GPSStatus;
    [SerializeField] Text latitudeValue;
    [SerializeField] Text longitudeValue;
    [SerializeField] Text altitudeValue;
    [SerializeField] Text horizontalAccuracyValue;
    [SerializeField] Text timeStampValue;

    [HideInInspector] public float latitude;
    [HideInInspector] public float longitude;
    [HideInInspector] public float altitude;
    [HideInInspector] public float horizontalAccuracy;
    [HideInInspector] public double timeStamp;
    [HideInInspector] public float heading;

    [SerializeField] Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        if (debugText) debugText.text = "DEBUGGING...";

        // Request permissions
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
#elif UNITY_IOS
        PlayerSettings.iOS.locationUsageDescription = "Details to use location";
#endif
        // Start GPS coroutine
        StartCoroutine(GPSLoc());
    }

    IEnumerator GPSLoc()
    {
        yield return new WaitForSeconds(0.3f);

        // If we are in the unity editor, wait until unity remote is connected
#if UNITY_EDITOR
        // Wait until Unity connects to the Unity Remote, while not connected, yield return null
        while (!UnityEditor.EditorApplication.isRemoteConnected)
        {
            yield return null;
        }
#endif

        // If the gps is not enabled, break the coroutine
        if (!Input.location.isEnabledByUser)
        {
            GPSStatus.text = "The locaiont service is offline";
            yield break;
        }
        if (GPSStatus) GPSStatus.text = "Initializing";
        yield return new WaitForSeconds(1);

        // Start service
        Input.compass.enabled = true;
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
            yield return new WaitForSeconds(3);
            while (Input.location.status == LocationServiceStatus.Running)
            {
                StartCoroutine(UpdateGPSData());
                StartCoroutine(Compass());
                yield return null;
            }
            GPSStatus.text = "Exited";
        }
    }

    IEnumerator UpdateGPSData()
    {
        //GPSStatus.text = (Input.location.status == LocationServiceStatus.Running).ToString();
        if (Input.location.status != LocationServiceStatus.Running)
        {
            GPSStatus.text = "Stop";
            yield return null;
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

        yield return null;
    }

    IEnumerator Compass()
    {
        Debug.Log("I AM HERE");
        debugText.text = Input.compass.enabled.ToString();
        if (!Input.compass.enabled) yield return null;
        heading = Input.compass.trueHeading;
        yield return null;
    }
}
