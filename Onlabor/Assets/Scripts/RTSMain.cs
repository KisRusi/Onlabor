using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSMain : MonoBehaviour
{
    private List<RtsUnit> selectedUnits;
    [SerializeField]private Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

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
            if(player.IsSelected)
            {
                player.MoveToDestination(MoveToClick.GetMousePosition());
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
                if(raycastHit.collider.GetComponent<Player>())
                {
                    player.IsSelected = true;
                    Debug.Log(player.IsSelected);
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
        player.IsSelected = false;
        selectedUnits.Clear();
    }
}
