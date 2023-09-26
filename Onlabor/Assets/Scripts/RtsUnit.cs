using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.HealthSystemCM;
using System;
using static BuildingManager;
using System.Linq;

public class RtsUnit : MonoBehaviour, IGetHealthSystem
{

    [SerializeField] private bool isEnemy;


    public enum State
    {
        Normal,
        GoingTo_Gathering,
        GoingBack_Gathering,
        WaitingForStorage,
        Gathering,
        MoveToTarget,
        Attacking,
    }

    private HealthSystem healthSystem;

    private bool isSelected;
    private float gatheringTime;
    private int resourceAmount;
    private State currentState;
    private RTSMain rtsMain;
    private GameObject selectedCircle;
    private ResourceNode resourceNode;
    private GameObject? resourceStorage;
    private NavMeshAgent navMeshAgent;
    private RtsUnit targetUnit;
    private float attackTime;


    private void Start()
    {
        
    }

    

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        selectedCircle = transform.Find("Selected").gameObject;
        currentState = State.Normal;
        resourceStorage = null;
        healthSystem = new HealthSystem(100);
        healthSystem.OnDead += HealthSystem_OnDead;
        rtsMain = GameObject.Find("RtsMain").GetComponent<RTSMain>();
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
        if (!isEnemy)
        {
            rtsMain.CheckForEnemeis(GetPosition(), 3f);
            switch (currentState)
            {
                case State.Normal:
                    AutomaticAttackInArea(rtsMain.GetEnemies());
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

                    CheckForResourceStorage();
                    if(resourceStorage == null)
                    {
                        currentState = State.WaitingForStorage;
                        goto case State.WaitingForStorage;
                    }
                    MoveToDestination(resourceStorage.transform.position);
                    reachDestination = 2f;
                    if (Vector3.Distance(transform.position,resourceStorage.transform.position) <  reachDestination)
                    {
                        
                        resourceAmount = 0;
                        currentState = State.GoingTo_Gathering;
                    }
                    break;
                case State.WaitingForStorage:
                    CheckForResourceStorage();
                    if(resourceStorage !=null)
                    {
                        currentState = State.GoingBack_Gathering;
                    }
                    break;
                case State.MoveToTarget:
                    if (targetUnit.IsDead())
                    {
                        MoveAndResetState(GetPosition());
                        //AutomaticAttackInArea(rtsMain.GetEnemies());
                    }
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
                    if(rtsMain.GetEnemies().Count == 0)
                        MoveAndResetState(GetPosition());
                    attackTime -= Time.deltaTime;
                    float attackTimerMax = 1f;
                    if(attackTime < 0) 
                    {
                        attackTime += attackTimerMax;
                        targetUnit.Damage(20);

                        Debug.Log(name + "attack");

                        if(targetUnit.IsDead())
                        {
                            rtsMain.GetEnemies().Remove(targetUnit);
                            AutomaticAttackInArea(rtsMain.GetEnemies());
                        }
                    }
                    break;


            }
        }
    }

    private void CheckForResourceStorage()
    {
        foreach(var storage in RTSMain.Instance.resourceStorages)
        {
            if(resourceStorage == null)
            {
                resourceStorage = storage;
                continue;
            }
            float distanceToCurrentStorage = Vector3.Distance(transform.position, resourceStorage.transform.position);
            float distanceToNextStorage = Vector3.Distance(transform.position,storage.transform.position);
            if(distanceToNextStorage < distanceToCurrentStorage)
            {
                resourceStorage = storage;
            }
        }
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

    public void AutomaticAttackInArea(List<RtsUnit> enemies)
    {
        var count = enemies.Count;
        var random = UnityEngine.Random.Range(0, count);
        if (count > 0)
        {
            SetTarget(enemies[random].transform.GetComponent<RtsUnit>());
            if (enemies[random].transform.GetComponent<RtsUnit>().IsDead())
            {
                rtsMain.RemoveEnemy(enemies[random]);
            }
        }
        
    }

}
