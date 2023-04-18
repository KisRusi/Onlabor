using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RtsUnit : MonoBehaviour
{
    public enum State
    {
        Normal,
        GoingTo_Gathering,
        GoingBack_Gathering,
        Gathering,
    }

    private bool isSelected;
    private float gatheringTime;
    private int resourceAmount;
    private State currentState;
    private GameObject selectedCircle;
    private ResourceNode resourceNode;
    private GameObject resourceStorage;
    private NavMeshAgent navMeshAgent;
    BuildingManager buildingManager;

    private void Start()
    {
        buildingManager.OnStoragePlaced += BuildingManager_OnStoragePlaced;

    }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        selectedCircle = transform.Find("Selected").gameObject;
        currentState = State.Normal;
        resourceStorage = GameObject.Find("ResourceStorage");
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        SetSelected(false);
    }
    private void Update()
    {

        
        
        
        switch(currentState)
        {
            case State.Normal:
                break;
            case State.GoingTo_Gathering:

                MoveToDestination(resourceNode.GetPosition());
                float reachDestination = 1f;
                if (Vector3.Distance(transform.position,resourceNode.GetPosition()) < reachDestination)
                {
                    navMeshAgent.ResetPath();
                    currentState = State.Gathering;
                }
                break;
            case State.Gathering:
                
                gatheringTime -= Time.deltaTime;
                if(gatheringTime < 0)
                {
                    float maxGatheringTime = 1f;
                    gatheringTime += maxGatheringTime;
                    
                    resourceAmount++;
                    Debug.Log("Gather!"+ resourceAmount);
                    int maxResourceAmount = 3;
                    if(resourceAmount > maxResourceAmount)
                    {
                        currentState = State.GoingBack_Gathering;
                    }
                }
                break;
            case State.GoingBack_Gathering:
                
                //Vector3 resourceStorage = new Vector3(1,0,0);
                MoveToDestination(resourceStorage.transform.position);
                reachDestination = 1f;
                if (Vector3.Distance(transform.position,resourceStorage.transform.position) <  reachDestination)
                {
                    
                    resourceAmount = 0;
                    currentState = State.GoingTo_Gathering;
                }


                break;
        }
    }

    private void BuildingManager_OnStoragePlaced(object sender, BuildingManager.OnStoragePlaceEventArgs e)
    {
        
        GameObject placedResourceStorage = e.gameObject;
        float distanceToStorage = Vector3.Distance(transform.position, resourceStorage.transform.position);
        float distanceToPlacedStorage = Vector3.Distance(transform.position,placedResourceStorage.transform.position);
        if (distanceToPlacedStorage < distanceToStorage)
            resourceStorage = placedResourceStorage;
        return;
    }

    public void MoveToDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
    }

    public void MoveAndResetState(Vector3 destination)
    {
        MoveToDestination(destination);
        currentState = State.Normal;
    }

    public void SetSelected(bool isSelected)
    {
        selectedCircle.SetActive(isSelected);
    }

    public void SetResourceNode(ResourceNode resourceNode)
    {
        this.resourceNode = resourceNode;
        currentState = State.GoingTo_Gathering;
    }

    
}
