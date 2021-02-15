using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaundryTaskAreaSpawner : MonoBehaviour
{
    private WorkStation[] workStations;

    void Start()
    {
        workStations = FindObjectsOfType<WorkStation>();

        GameObject laundryTaskAreaPrefab = (GameObject) Resources.Load("LaundryTaskArea");
        GameObject tableAreaPrefab = (GameObject)Resources.Load("TableArea");
        for(int i = 0; i < workStations.Length; i++) {
            //Instantiate the appropriate laundry task area prefab at the appropriate location
            GameObject prefab;
            if (workStations[i] is TableWorkstation) {
                prefab = tableAreaPrefab;
            }
            else
                prefab = laundryTaskAreaPrefab;
            GameObject laundryTaskArea = Instantiate(prefab, new Vector3(300.0f, 0.0f, 0.0f), Quaternion.identity);
            laundryTaskArea.SetActive(false);
            workStations[i].laundryTaskArea = laundryTaskArea;
            workStations[i].Initialize();
        }
    }
}
