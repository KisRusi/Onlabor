using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BuildingManager;

public class ResourceStorage : MonoBehaviour
{

    private float coolDown;
    public enum State
    {
        Idle,
        Building,
        Ready
    }
    private State currentState;
    [SerializeField]
    private Material readyMaterial;
    private void Awake()
    {
        coolDown = 5f;
    }

    public event EventHandler<OnStorageReadyEventArgs> OnStorageReady;

    public class OnStorageReadyEventArgs : EventArgs
    {
        public GameObject gameObject;
    }

    private void Update()
    {
        switch(currentState)
        {
            case State.Idle:
                break;
            case State.Building:
                GetComponentInChildren<CanvasScaler>(true).gameObject.SetActive(true);
                break;
            case State.Ready:
                OnStorageReady?.Invoke(this, new OnStorageReadyEventArgs { gameObject = this.gameObject});
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
