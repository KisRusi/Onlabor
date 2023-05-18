using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public  class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject pendingObject;
    private Vector3 position;
    private RaycastHit hit;
    public bool canPlace = true;
    private bool isStorage = false;
    

    public event EventHandler<OnStoragePlaceEventArgs> OnStoragePlaced;
    public class OnStoragePlaceEventArgs :EventArgs
    {
        public GameObject gameObject;
    }
 

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Material[] materials;

 
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
            OnStoragePlaced?.Invoke(this, new OnStoragePlaceEventArgs { gameObject = pendingObject.gameObject });
            pendingObject.GetComponent<ResourceStorage>().SwitchState(ResourceStorage.State.Building);
            pendingObject.GetComponentInChildren<LoadingCircleUIStorage>(true).SetReadyTime(Time.time);
            
        }
        else
        {
            GameObject.Find("RtsMain").GetComponent<RTSMain>().AddBarrack(pendingObject);
            pendingObject.GetComponent<Barrack>().SwitchState(Barrack.State.Building);
            pendingObject.GetComponentInChildren<LoadingCircleUIBarrack>(true).SetReadyTime(Time.time);
        }
        
        
        pendingObject = null;
        isStorage = false;
    }

    public void SelectObject(int index)
    {
        if(index == 0)
        {
            isStorage = true;
        }
        pendingObject = Instantiate(objects[index], position, transform.rotation);
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
