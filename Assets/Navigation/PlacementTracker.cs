using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementTracker : MonoBehaviour
{
    private ARRaycastManager rayManager;
    public GameObject visual;

    void Start()
    {
        // get the components
        rayManager = FindObjectOfType<ARRaycastManager>();
        // hide the placement visual
        visual.SetActive(false);
    }

    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // if we hit ar plane, update pos and rot
        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;

            if (!visual.activeInHierarchy)
                visual.SetActive(true);
        }
        else
        {
            visual.SetActive(false);
        }
    }
}
