using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public  class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public GameObject[] objects;
    private GameObject pendingObject;
    private Vector3 position;
    private RaycastHit hit;
    public bool canPlace = true;
    private bool isStorage = false;
    private bool isBarrack = false;
    

    public event EventHandler<OnStoragePlaceEventArgs> OnStoragePlaced;
    public class OnStoragePlaceEventArgs :EventArgs
    {
        public GameObject gameObject;
    }
 

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Material[] materials;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
         
        if(pendingObject != null)
        {
            pendingObject.transform.position = position;
            UpdateMaterials();
            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceObject();
                return;
            }
            
        }
        

    }
    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            position = hit.point;
           
        }
        
    }

    public void PlaceObject()
    {
        pendingObject.GetComponent<MeshRenderer>().material = materials[3];
        if (isStorage)
        {
            pendingObject.GetComponent<ResourceStorage>().OnStorageReady += OnStorageReady;
            pendingObject.GetComponent<ResourceStorage>().SwitchState(ResourceStorage.State.Building);
            pendingObject.GetComponentInChildren<LoadingCircleUIStorage>(true).SetReadyTime(Time.time);
            
        }
        else if(isBarrack)
        {
            GameObject.Find("RtsMain").GetComponent<RTSMain>().AddBarrack(pendingObject);
            pendingObject.GetComponent<Barrack>().SwitchState(Barrack.State.Building);
            pendingObject.GetComponentInChildren<LoadingCircleUIBarrack>(true).SetReadyTime(Time.time);
        }
        else
        {
            pendingObject.GetComponent<Turret>().SwitchState(Turret.State.Building);
            pendingObject.GetComponentInChildren<LoadingCircleUITurret>(true).SetReadyTime(Time.time);
        }


        pendingObject = null;
        isStorage = false;
        isBarrack = false;
    }

    private void OnStorageReady(object sender, ResourceStorage.OnStorageReadyEventArgs e)
    {
        OnStoragePlaced?.Invoke(this, new OnStoragePlaceEventArgs { gameObject = e.gameObject });
    }

    public void SelectObject(int index)
    {
        if(index == 0)
        {
            isStorage = true;
        }
        if(index == 1)
        {
            isBarrack = true;
        }
        Vector3 pos = new Vector3 { x = position.x, y = 0.5f, z = position.z};
        pendingObject = Instantiate(objects[index], pos, transform.rotation);
    }

    void UpdateMaterials()
    {
        if (canPlace)
        {
            pendingObject.GetComponent<MeshRenderer>().material = materials[0];
        }
        else
        {
            pendingObject.GetComponent<MeshRenderer>().material = materials[1];
        }

    }
    
}
