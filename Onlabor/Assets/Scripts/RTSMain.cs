using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RTSMain : MonoBehaviour
{

    public static RTSMain Instance { get; private set; }

    private List<RtsUnit> selectedUnits;
    private List<GameObject> barracks;
    public List<GameObject> resourceStorages;
    private Barrack selectedBarrack;
    [SerializeField]
    private Player player;

    [SerializeField]
    private Transform selectedArea = null;

    [SerializeField]
    private Transform barrackPanel = null;

    private Vector3 startpos;
    
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        BuildingManager.Instance.OnStoragePlaced += OnStorage_Placed;
    }

    

    private void Awake()
    {
        Instance = this;
        selectedUnits = new List<RtsUnit>();
        barracks = new List<GameObject>();
        resourceStorages = new List<GameObject>();
        selectedBarrack = null;
        selectedArea.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
            {
                if (raycastHit.collider.TryGetComponent<ResourceNode>(out ResourceNode resourceNode))
                {
                    foreach (var unit in selectedUnits)
                    {
                        unit.SetResourceNode(resourceNode);
                    }
                }
                else
                {
                    if (raycastHit.collider.TryGetComponent<RtsUnit>(out RtsUnit targetUnit))
                    {
                        if (targetUnit.IsEnemy())
                        {
                            foreach (var unit in selectedUnits)
                            {
                                unit.SetTarget(targetUnit);
                                //enemies = CheckForEnemeis(raycastHit.point, 3f);
                            }
                        }

                    }
                    else
                    {
                        foreach (var unit in selectedUnits)
                        {
                            unit.MoveAndResetState(MoveToClick.GetMousePosition());
                        }
                        if (player.IsSelected)
                        {
                            player.MoveToDestination(MoveToClick.GetMousePosition());
                        }
                    }
                }
            }
            
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            selectedArea.gameObject.SetActive(true);
            startpos = MoveToClick.GetMousePosition();
            if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject == GameObject.Find("UnitSpawn") || EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject == GameObject.Find("Spawn"))
            {
                barrackPanel.gameObject.SetActive(true);
                return;
            }
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit2) )
            {
                if (raycastHit2.collider.TryGetComponent<Barrack>(out Barrack barrack) && barrack.GetState()== Barrack.State.Idle)
                {
                    
                    barrackPanel.gameObject.SetActive(true);
                    selectedBarrack = barrack;
                    selectedBarrack.GetComponentInChildren<SpawnMarker>(true).gameObject.SetActive(true);
                    return;
                }
                
            }

            if(selectedBarrack != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit3))
            {
                if(raycastHit3.collider.TryGetComponent<SpawnMarker>(out SpawnMarker marker))
                {
                    barrackPanel.gameObject.SetActive(true);
                    return;
                }
                else
                {
                    selectedBarrack = null;
                }
            }
            


            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit1) && Input.GetKey(KeyCode.LeftShift))
            {
                if (raycastHit1.collider.TryGetComponent<RtsUnit>(out RtsUnit unit))
                {
                    if(!unit.IsEnemy())
                    {
                        unit.SetSelected(true);
                        selectedUnits.Add(unit);
                    }
                }
                if(raycastHit1.collider.GetComponent<Player>())
                {
                    player.IsSelected = true;
                }
                if(raycastHit1.collider.TryGetComponent<RangeUnit>(out RangeUnit rangeUnit))
                {
                    rangeUnit.SetSelected(true);
                    selectedUnits.Add(rangeUnit);
                }
                return;
            }

            
            
            UnSelectUnits();
            barrackPanel.gameObject.SetActive(false);
        }


        if(Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit3))
            {
                if (raycastHit3.collider.TryGetComponent<SpawnMarker>(out SpawnMarker marker))
                {
                    marker.SetActive(true);
                    selectedArea.gameObject.SetActive(false);
                }

            }
            

            Vector3 currentPos = MoveToClick.GetMousePosition();
            Vector3 lowerLeft = new Vector3(Mathf.Min(startpos.x, currentPos.x), 0.1f, Mathf.Min(startpos.z, currentPos.z));
            Vector3 upperRight = new Vector3(Mathf.Max(startpos.x, currentPos.x), 0.1f, Mathf.Max(startpos.z, currentPos.z));

            Vector3 boxCenter = (lowerLeft + upperRight) / 2;
            selectedArea.position = boxCenter;

            selectedArea.localScale = upperRight - lowerLeft;
            
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
                    if(!unit.IsEnemy())
                    {
                        unit.SetSelected(true);
                        selectedUnits.Add(unit);
                    }
                }
                if(col.GetComponent<Player>())
                {
                    player.IsSelected = true;
                
                }
            }
        }

        
    }

    private void OnStorage_Placed(object sender, BuildingManager.OnStoragePlaceEventArgs e)
    {
        resourceStorages.Add(e.gameObject);
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

    public void AddBarrack(GameObject barrack)
    {
        barracks.Add(barrack);
    }

    public Barrack GetSelectedBarrack()
    {
        return selectedBarrack;
    }
    
    
}
