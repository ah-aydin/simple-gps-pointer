using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class Spawner : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject objectToSpawnPrefab;
    GameObject objectToSpawn;
    ARPlaneManager arPlaneManager;
    PlacementTracker placementTracker = null;

    private void Start()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        placementTracker = FindObjectOfType<PlacementTracker>();
        objectToSpawn = Instantiate(objectToSpawnPrefab, Vector3.zero, objectToSpawnPrefab.transform.rotation);
    }

    public void Spawn()
    {
        bool placement_active = placementTracker.visual.activeSelf;
        if (placement_active)
        {
            objectToSpawn.transform.position = placementTracker.gameObject.transform.position;
            objectToSpawn.transform.rotation = placementTracker.gameObject.transform.rotation;
        }

        if (objectToSpawn.activeSelf)
        {
            objectToSpawn.SetActive(false);
            return;
        }

        if (!placement_active)
            return;

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = placementTracker.gameObject.transform.position;
        objectToSpawn.transform.rotation = placementTracker.gameObject.transform.rotation;
    }
}
