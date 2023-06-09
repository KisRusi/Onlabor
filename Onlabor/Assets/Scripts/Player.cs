using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.HealthSystemCM;
using System;
using System.Linq;
using UnityEngine.UI;
using Unity.Netcode;

public class Player : NetworkBehaviour, IGetHealthSystem
{
    public static event EventHandler OnAnyPlayerSpawned;
    public static Player LocalInstance { get; private set; }

    private NavMeshAgent navMeshAgent;
    private bool isSelected;
    private GameObject selectedCircle;
    private GameObject areaDamageCircle;
    private GameObject healArea;
    private HealthSystem healthSystem;
    private List<GameObject> targetUnits;
    private float time = 0;
    private Vector3 startpos;

    [SerializeField]
    private Button btnArea;
    [SerializeField]
    private Button btnHeal;

    [SerializeField]
    private Transform selectedArea = null;


    //[SerializeField]
    //private LayerMask layerMask;

    public enum AbilityState
    {
        Idle,
        Ability1,
        Ability2,
        Ability3,
    }

    public AbilityState abilityState
    {
        set;
        get;
    }

    public bool IsSelected
    {
        set
        {
            isSelected = value;
            selectedCircle.SetActive(value);
        }
        get { return isSelected; }
    }

    public float NextAreaDamageTime
    {
        set;
        get;
    }
    public float NextHealTime
    {
        set;
        get;
    }
    public bool IsInHealArea
    {
        set;
        get;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsOwner)
        {
            LocalInstance = this;
        }
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        selectedCircle = transform.Find("Selected").gameObject;
        areaDamageCircle = transform.Find("AreaDamageCircle").gameObject;
        healArea = transform.Find("HealArea").gameObject;
        selectedArea.gameObject.SetActive(false);
        healthSystem = new HealthSystem(200);
        SetSelected(false);
        targetUnits = new List<GameObject>();
        healthSystem.OnDead += HealthSystem_OnDead;
        NextAreaDamageTime = 0;
        Damage(100);
    }

    

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
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


    // Update is called once per frame
    private void Update()
    {
        //time = Time.time;
        //if (Time.time > NextAreaDamageTime)
        //{
        //    btnArea.interactable = true;
        //}
        //if(Time.time > NextHealTime)
        //{
        //    btnHeal.interactable = true;
        //}


        switch (abilityState)
        {
            case AbilityState.Idle:
                break;
            case AbilityState.Ability1:
                areaDamageCircle.SetActive(true);
                break;
            case AbilityState.Ability2:
                healArea.SetActive(true);
                break;
            case AbilityState.Ability3:
                Ability3();
                break;
        }
    }

    

    public void MoveToDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
    }

    public void SetSelected(bool isSelected)
    {
        selectedCircle.SetActive(isSelected);
    }

    public void Damage(int amount)
    {
        healthSystem.Damage(amount);
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    public void SwitchState(AbilityState state)
    {
        abilityState = state;
        IsSelected = true;
    }

    public void Ability2()
    {
        //TODO
        abilityState = AbilityState.Idle;
    }

    public void Ability3()
    {
        //TODO
        abilityState = AbilityState.Idle;
    }

    public void AddTarget(GameObject targetUnit)
    {
        targetUnits.Add(targetUnit);
    }

    public void RemoveTarget(GameObject targetUnit)
    {
        targetUnits.Remove(targetUnit);
    }

    public List<GameObject> GetTargetUnits()
    {
        var tmpList = new List<GameObject>();
        foreach (var unit in targetUnits)
        {
            tmpList.Add(unit);
        }
        return tmpList;
    }

    public void ClearTargetUnits()
    {
        targetUnits.Clear();
    }

}
