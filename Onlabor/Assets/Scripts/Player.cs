using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.HealthSystemCM;
using System;
using System.Linq;
using UnityEngine.UI;

public class Player : MonoBehaviour, IGetHealthSystem
{

    private NavMeshAgent navMeshAgent;
    private bool isSelected;
    private GameObject selectedCircle;
    private GameObject areaDamageCircle;
    private HealthSystem healthSystem;
    private List<GameObject> targetUnits;
    private float time = 0;

    [SerializeField]
    private Button btn;


    [SerializeField]
    private LayerMask layerMask;

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

    public bool IsSelected{
        set { isSelected = value;
            selectedCircle.SetActive(value);
        }
        get { return isSelected; }
        }

    public float NextAreaDamageTime
    {
        set;
        get;
    }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        selectedCircle = transform.Find("Selected").gameObject;
        areaDamageCircle = transform.Find("AreaDamageCircle").gameObject;
        healthSystem = new HealthSystem(200);
        SetSelected(false);
        targetUnits = new List<GameObject>();
        healthSystem.OnDead += HealthSystem_OnDead;
        NextAreaDamageTime = 0;
        
    }

    

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }


    // Update is called once per frame
    private void Update()
    {
        time = Time.time;
        if (Time.time > NextAreaDamageTime)
        {
            btn.interactable = true;
        }
        switch (abilityState)
        {
            case AbilityState.Idle:
                break;
            case AbilityState.Ability1:
                areaDamageCircle.SetActive(true);
                break;
            case AbilityState.Ability2:
                Ability2();
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
