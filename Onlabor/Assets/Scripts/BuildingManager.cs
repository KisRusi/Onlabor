using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject pendingObject;
    private Vector3 position;
    private RaycastHit hit;
    public bool canPlace = true;
    

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
        pendingObject.GetComponent<MeshRenderer>().material = materials[2];
        pendingObject = null;
    }

    public void SelectObject(int index)
    {
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
