using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BaseBuilding : MonoBehaviour
{
    public static BaseBuilding Instance { get; private set; }
    [SerializeField]
    private GameObject unitToSpawn;
    private Vector3 pos;

    public void Awake()
    {
        Instance = this;
    }
    public void SpawnUnit()
    {
        pos = transform.localPosition;
        var spawnpos = GetComponentInChildren<SpawnMarker>(true).gameObject.transform.position;
        Instantiate(unitToSpawn, spawnpos, transform.rotation);

    }
}
