using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    
    public GameObject prefab;
    BuildingManager buildingManager;
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
        coolDown = 5f;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                RTSMain.Instance.CheckForEnemeis(transform.position, 4f);
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
