using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSMain : MonoBehaviour
{
    private List<RtsUnit> selectedUnits;
    [SerializeField]private Player player;

    [SerializeField]private Transform selectedArea = null;
    private Vector3 startpos;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Awake()
    {
        selectedUnits = new List<RtsUnit>();
        selectedArea.gameObject.SetActive(false);
        
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

        if (Input.GetMouseButtonDown(0))
        {
            selectedArea.gameObject.SetActive(true);
            startpos = MoveToClick.GetMousePosition();
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
                
                }
            }
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 currentPos = MoveToClick.GetMousePosition();
            Vector3 lowerLeft = new Vector3(Mathf.Min(startpos.x, currentPos.x) , 0.1f, Mathf.Min(startpos.z, currentPos.z));
            Vector3 upperRight = new Vector3(Mathf.Max(startpos.x, currentPos.x), 0.1f, Mathf.Max(startpos.z, currentPos.z));

            Vector3 boxCenter = (lowerLeft + upperRight) /2 ;
            selectedArea.position = boxCenter;
            
            selectedArea.localScale = upperRight-lowerLeft;
            


        }

        if(Input.GetMouseButtonUp(0))
        {
            selectedArea.gameObject.SetActive(false);
            Vector3 currentPos = MoveToClick.GetMousePosition();
            Vector3 lowerLeft = new Vector3(Mathf.Min(startpos.x, currentPos.x) , 0.1f, Mathf.Min(startpos.z, currentPos.z));
            Vector3 upperRight = new Vector3(Mathf.Max(startpos.x, currentPos.x), 0.1f, Mathf.Max(startpos.z, currentPos.z));

            Vector3 boxCenter = (lowerLeft + upperRight) /2 ;
            selectedArea.position = boxCenter;
            
            selectedArea.localScale = upperRight-lowerLeft;
            Vector3 halfExtents = new Vector3((upperRight.x - lowerLeft.x) * 0.5f,
                                                1,
                                                (upperRight.z - lowerLeft.z) * 0.5f);

            Collider[] colliiderArray = Physics.OverlapBox(boxCenter, halfExtents);
            foreach(Collider col in colliiderArray)
            {
                if (col.TryGetComponent<RtsUnit>(out RtsUnit unit))
                {
                    unit.SetSelected(true);
                    selectedUnits.Add(unit);
                }
                if(col.GetComponent<Player>())
                {
                    player.IsSelected = true;
                
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
