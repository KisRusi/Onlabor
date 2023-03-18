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

        if (Input.GetMouseButtonDown(1))
        {
            foreach (var unit in selectedUnits) 
            {
                unit.MoveToDestination(MoveToClick.GetMousePosition());
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            UnSelectUnits();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
            {
                if (raycastHit.collider.TryGetComponent<RtsUnit>(out RtsUnit unit))
                {
                    unit.SetSelected(true);
                    selectedUnits.Add(unit);
                }
            }
        }
    }

    private void UnSelectUnits()
    {
        foreach(var unit in selectedUnits)
        {
            unit.SetSelected(false);
        }
        selectedUnits.Clear();
    }
}
