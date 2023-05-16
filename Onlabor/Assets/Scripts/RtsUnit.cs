using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.HealthSystemCM;
using System;

public class RtsUnit : MonoBehaviour, IGetHealthSystem
{

    [SerializeField] private bool isEnemy;

    public enum State
    {
        Normal,
        GoingTo_Gathering,
        GoingBack_Gathering,
        Gathering,
        MoveToTarget,
        Attacking,
    }

    private HealthSystem healthSystem;

    private bool isSelected;
    private float gatheringTime;
    private int resourceAmount;
    private State currentState;
    private GameObject selectedCircle;
    private ResourceNode resourceNode;
    private GameObject resourceStorage;
    private NavMeshAgent navMeshAgent;
    BuildingManager buildingManager;
    private RtsUnit targetUnit;
    private float attackTime;


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
        healthSystem = new HealthSystem(100);
        healthSystem.OnDead += HealthSystem_OnDead;
        if (this.IsEnemy())
            return;
        SetSelected(false);
        Damage(70);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    private void Update()
    {

        switch(currentState)
        {
            case State.Normal:
                break;
            case State.GoingTo_Gathering:
                Debug.Log("going gathering");
                MoveToDestination(resourceNode.GetPosition());
                float reachDestination = 2f;
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

                
                MoveToDestination(resourceStorage.transform.position);
                reachDestination = 2f;
                if (Vector3.Distance(transform.position,resourceStorage.transform.position) <  reachDestination)
                {
                    
                    resourceAmount = 0;
                    currentState = State.GoingTo_Gathering;
                }
                break;
            case State.MoveToTarget:
                if (targetUnit.IsDead())
                    MoveAndResetState(GetPosition());
                else
                {
                    MoveToDestination(targetUnit.GetPosition());
                    reachDestination = 1f;
                    if (Vector3.Distance(transform.position, targetUnit.transform.position) < reachDestination)
                    {
                        currentState = State.Attacking;
                    }
                }
                
                break;
            case State.Attacking:
                attackTime -= Time.deltaTime;
                float attackTimerMax = 1f;
                if(attackTime < 0) 
                {
                    attackTime += attackTimerMax;
                    Debug.Log("Attacking");
                    targetUnit.Damage(20);
                    if(targetUnit.IsDead())
                    {
                        MoveAndResetState(GetPosition());
                    }
                }
                break;


        }
    }

    private void BuildingManager_OnStoragePlaced(object sender, BuildingManager.OnStoragePlaceEventArgs e)
    {
        
        if (isEnemy)
            return;
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

    public void Damage(float amount)
    {
        if (IsDead())
            return;
        healthSystem.Damage(amount);
    }

    public void Heal(float amount)
    {
        if (healthSystem.GetHealth() >= healthSystem.GetHealthMax())
            return;
        healthSystem.Heal(amount);
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void SetTarget(RtsUnit targetUnit)
    {
        this.targetUnit = targetUnit;
        currentState = State.MoveToTarget;
    }    

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }
}
