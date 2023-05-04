using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : MonoBehaviour
{
    [SerializeField] 
    private GameObject unitToSpawn;

    public GameObject prefab;
    BuildingManager buildingManager;
    private Vector3 pos;



    private void Start()
    {
        buildingManager.OnBarrackPlaced += BuildingManager_OnBarrackPlaced;
    }
    private void Awake()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }

    public void SpawnUnit()
    {
        Debug.Log("posstart" + pos);
        GameObject spawnedUnit = Instantiate(unitToSpawn,pos, Quaternion.identity);
        Debug.Log(pos);
        spawnedUnit.transform.position = pos;
        Debug.Log(spawnedUnit.transform.position);
    }

    private void BuildingManager_OnBarrackPlaced(object sender, BuildingManager.OnBarrackPlaceEventArgs e)
    { 
        pos = e.gameObject.transform.position;
        Debug.Log(pos);
    }

}
