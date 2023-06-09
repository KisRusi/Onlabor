using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RTSMain : NetworkBehaviour
{
    private List<RtsUnit> selectedUnits;
    private List<GameObject> barracks;
    private Barrack selectedBarrack;
    
    private Player player;

    [SerializeField]
    private Transform selectedArea = null;

    [SerializeField]
    private Transform barrackPanel = null;

    private List<RtsUnit> enemies;

    private Vector3 startpos;
    
    private void Start()
    {
        if(Player.LocalInstance != null)
        {
            player = Player.LocalInstance;
        }else
        {
            Player.OnAnyPlayerSpawned -= Player_OnAnyPlayerSpawned;
            Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        }
    }

    private void Player_OnAnyPlayerSpawned(object sender, EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            player = Player.LocalInstance;
        }
    }

    private void Awake()
    {
        selectedUnits = new List<RtsUnit>();
        barracks = new List<GameObject>();
        selectedBarrack = null;
        selectedArea.gameObject.SetActive(false);
        enemies = new List<RtsUnit>();
        //player = FindAnyObjectByType<Player>().gameObject.GetComponent<Player>();
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
                    Debug.Log("resourcenode");
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
                                enemies = CheckForEnemeis(raycastHit.point, 4f);
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
            if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject == GameObject.Find("UnitSpawn"))
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
                    return;
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
                if (raycastHit1.collider.GetComponent<Player>())
                {

                    player.IsSelected = true;

                }
                return;
            }
            
            UnSelectUnits();
            barrackPanel.gameObject.SetActive(false);
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
                    if(!unit.IsEnemy())
                    {
                        unit.SetSelected(true);
                        selectedUnits.Add(unit);
                    }
                }
                if (col.GetComponent<Player>())
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

    public void AddBarrack(GameObject barrack)
    {
        barracks.Add(barrack);
    }

    public Barrack GetSelectedBarrack()
    {
        return selectedBarrack;
    }

    public List<RtsUnit> CheckForEnemeis(Vector3 position, float radius)
    {
        var colliders = Physics.OverlapSphere(position, radius);
        foreach(var collider in colliders)
        {
            if (collider.transform.gameObject.name.Contains("Enemy")&& !enemies.Contains(collider.gameObject.GetComponent<RtsUnit>()))
                enemies.Add(collider.transform.gameObject.GetComponent<RtsUnit>());
        }
        return enemies;
    }

    public void RemoveEnemy(RtsUnit deadEnemy)
    {
        enemies.Remove(deadEnemy);
    }

    public List<RtsUnit> GetEnemies()
    {
        return enemies;
    }
    
}
