using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSMain : MonoBehaviour
{
    private List<RtsUnit> selectedUnits;

    private void Awake()
    {
        selectedUnits = new List<RtsUnit>();
    }
    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
        //    {
        //        Debug.Log(raycastHit.collider);
        //    }
        //}
    }
}
