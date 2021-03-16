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
        GameObject washingMachineAreaPrefab = (GameObject)Resources.Load("WashingMachineArea");
        GameObject dryerAreaPrefab = (GameObject)Resources.Load("DryerArea");
        GameObject ironingBoardPrefab = (GameObject)Resources.Load("IroningBoardArea");
        GameObject clotheslinePrefab = (GameObject)Resources.Load("ClotheslineArea");
        GameObject washTubPrefab = (GameObject)Resources.Load("WashTubArea");
        GameObject sewingMachinePrefab = (GameObject)Resources.Load("SewingMachineArea");

        for (int i = 0; i < workStations.Length; i++) {
            //Instantiate the appropriate laundry task area prefab at the appropriate location
            GameObject prefab;

            if (workStations[i] is TableWorkstation) {
                prefab = tableAreaPrefab;
            }else if(workStations[i] is WashingMachine) {
                prefab = washingMachineAreaPrefab;
            }
            else if (workStations[i] is Dryer) {
                prefab = dryerAreaPrefab;
            }
            else if (workStations[i] is IroningBoard) {
                prefab = ironingBoardPrefab;
            }
            else if(workStations[i] is Clothesline) {
                prefab = clotheslinePrefab;
            }
            else if (workStations[i] is WashTub) {
                prefab = washTubPrefab;
            }
            else if (workStations[i] is SewingMachine) {
                prefab = sewingMachinePrefab;
            }
            else
                prefab = laundryTaskAreaPrefab;

            GameObject laundryTaskArea = Instantiate(prefab, new Vector3(300.0f, 0.0f, 0.0f), Quaternion.identity, workStations[i].transform);

            laundryTaskArea.SetActive(false);
            workStations[i].laundryTaskArea = laundryTaskArea;
            workStations[i].Initialize();
        }
    }
}
