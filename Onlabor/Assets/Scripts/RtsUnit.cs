using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.HealthSystemCM;
using System;
using UnityEngine.UI;

public class RtsUnit : MonoBehaviour, IGetHealthSystem
{

    [SerializeField] protected bool isEnemy;


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
    private RTSMain rtsMain;
    private GameObject selectedCircle;

    protected GameObject? resourceStorage;
    protected float gatheringTime;
    protected int resourceAmount;
    protected ResourceNode resourceNode;
    protected NavMeshAgent navMeshAgent;
    protected State currentState;
    protected RtsUnit targetUnit;
    protected float attackTime;
    protected List<RtsUnit> enemies;


    private void Start()
    {
        
    }

    

    protected void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        selectedCircle = transform.Find("Selected").gameObject;
        currentState = State.Normal;
        resourceStorage = GameObject.Find("ResourceStorage");
        enemies = new List<RtsUnit>();
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
        Destroy(gameObject.GetComponent<SphereCollider>());
    }

    private void Update()
    {
        if (!isEnemy)
        {
            ChangeHealthBarColor();
            switch (currentState)
            {
                case State.Normal:
                    AutomaticAttackInArea(transform.position, 3f, 1f);
                    break;
                case State.MoveToTarget:
                    if (targetUnit.IsDead())
                    {
                        MoveAndResetState(GetPosition());
                    }
                    else
                    {
                        MoveToDestination(targetUnit.GetPosition());
                        float reachDestination = 1f;
                        if (Vector3.Distance(transform.position, targetUnit.transform.position) < reachDestination)
                        {
                            currentState = State.Attacking;
                        }
                    }
                    
                    break;
                case State.Attacking:
                    if(enemies.Count == 0)
                        MoveAndResetState(GetPosition());
                    attackTime -= Time.deltaTime;
                    float attackTimerMax = 1f;
                    if (targetUnit.IsDead())
                    {
                        AutomaticAttackInArea(transform.position, 3f, 1f);
                    }
                    if (attackTime < 0) 
                    {
                        attackTime += attackTimerMax;
                        targetUnit.Damage(20);
                    }
                    break;


            }
        }
    }

    public void CheckForResourceStorage()
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

    public void AutomaticAttackInArea(Vector3 position, float radius, float reachDestination)
    {
        enemies.Clear();
        enemies = CheckForEnemeis(position, radius);
        var count = enemies.Count;
        var random = UnityEngine.Random.Range(0, count);
        if (count > 0)
        {
            SetTarget(enemies[random].transform.GetComponent<RtsUnit>());
            currentState = State.Attacking;
            Vector3 dir = (targetUnit.GetPosition() - transform.position).normalized;
            if (Vector3.Distance(transform.position, targetUnit.transform.position) > 6f)
            {
                navMeshAgent.SetDestination(transform.position + (dir * 1.5f));
            }
        }
        
    }
    public List<RtsUnit> CheckForEnemeis(Vector3 position, float radius)
    {
        var colliders = Physics.OverlapSphere(position, radius);
        foreach (var collider in colliders)
        {
            if (collider.transform.gameObject.name.Contains("Enemy") && !enemies.Contains(collider.gameObject.GetComponent<RtsUnit>()))
                enemies.Add(collider.transform.gameObject.GetComponent<RtsUnit>());
        }
        return enemies;
    }

    public void ChangeHealthBarColor()
    {
        var images = gameObject.GetComponentInChildren<HealthBarUI>().GetComponentsInChildren<Image>();
        GameObject healthbar;
        float healthpercentage = healthSystem.GetHealth() / healthSystem.GetHealthMax();
        foreach (var image in images)
        {
            if (image.name.Contains("Bar"))
            {
                healthbar = image.gameObject;
                if (healthpercentage >= 0.7f)
                {
                    healthbar.GetComponent<Image>().color = new Color(0, 1, 0);
                }
                else if (healthpercentage >= 0.5f && healthpercentage < 0.7f)
                {
                    healthbar.GetComponent<Image>().color = new Color(1, 1, 0);
                }
                else if (healthpercentage >= 0.3f && healthpercentage < 0.5f)
                {
                    healthbar.GetComponent<Image>().color = new Color(1, 0.64f, 0);
                }
                else
                    return;
            }   
        } 
    }
}
