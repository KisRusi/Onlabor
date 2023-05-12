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

    private Barrack selectedBarrack;



    private void Awake()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        selectedBarrack = null;
    }


    public void SpawnUnit()
    {
        pos = transform.localPosition;
        var brck = GameObject.Find("RtsMain").GetComponent<RTSMain>().GetSelectedBarrack();
        GameObject spawnedUnit = Instantiate(unitToSpawn,brck.transform.position, transform.rotation);

    }


    public void SetBarrack(Barrack barrack)
    {
        selectedBarrack = barrack;
    }

}
