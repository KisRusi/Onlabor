using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrack : MonoBehaviour
{
    [SerializeField] 
    private GameObject unitToSpawn;

    public GameObject prefab;
    BuildingManager buildingManager;
    private Vector3 pos;

    private Barrack? selectedBarrack;
    private float coolDown;
    private State currentState;
    [SerializeField]
    private Material readyMaterial;

    public enum State
    {
        Idle,
        Building,
        Ready
    }


    private void Awake()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        selectedBarrack = null;
        coolDown = 5f;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Building:
                GetComponentInChildren<CanvasScaler>(true).gameObject.SetActive(true);
                break;
            case State.Ready:
                gameObject.GetComponent<MeshRenderer>().material = readyMaterial;
                SwitchState(State.Idle);
                break;
        }
    }

    public void SpawnUnit()
    {
        pos = transform.localPosition;
        var brck = RTSMain.Instance.GetSelectedBarrack();
        var spawnpos = brck.GetComponentInChildren<SpawnMarker>(true).gameObject.transform.position;
        Instantiate(unitToSpawn,spawnpos, transform.rotation);

    }

    public void SetSpawnMarker()
    {
        var brck = RTSMain.Instance.GetSelectedBarrack();
        brck.GetComponentInChildren<SpawnMarker>(true).gameObject.SetActive(true);
        brck.GetComponentInChildren<SpawnMarker>(true).IsActive = true;

    }


    public void SetBarrack(Barrack barrack)
    {
        selectedBarrack = barrack;
    }

    public float GetCoolDown()
    {
        return coolDown;
    }

    public void SwitchState(State state)
    {
        currentState = state;
    }

    public State GetState()
    {
        return currentState;
    }

}
