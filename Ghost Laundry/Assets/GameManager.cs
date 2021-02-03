using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private WorkStation[] workStations;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.sceneCount < 2)
            SceneManager.LoadScene("LaundryTasks", LoadSceneMode.Additive);

        workStations = FindObjectsOfType<WorkStation>();

        GameObject laundryTaskAreaPrefab = (GameObject) Resources.Load("LaundryTaskArea");
        for(int i = 0; i < workStations.Length; i++) {
            //Instantiate the appropriate laundry task area prefab at the appropriate location
            GameObject laundryTaskArea = Instantiate(laundryTaskAreaPrefab, new Vector3(300.0f, 0.0f, 0.0f), Quaternion.identity);
            workStations[i].laundryTaskArea = laundryTaskArea;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
