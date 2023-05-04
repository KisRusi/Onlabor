using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.HealthSystemCM;
using System;

public class Player : MonoBehaviour, IGetHealthSystem
{

    private NavMeshAgent navMeshAgent;
    private bool isSelected;
    private GameObject selectedCircle;
    private HealthSystem healthSystem;
    private AbilityState abilityState;

    public enum AbilityState
    {
        Idle,
        Ability1,
        Ability2,
        Ability3,
    }
    public bool IsSelected{
        set { isSelected = value;
            selectedCircle.SetActive(value);
        }
        get { return isSelected; }
        }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        selectedCircle = transform.Find("Selected").gameObject;
        healthSystem = new HealthSystem(200);
        SetSelected(false);

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }


    // Update is called once per frame
    private void Update()
    {
        switch(abilityState)
        {
            case AbilityState.Idle:
                break;
            case AbilityState.Ability1:
                Ability1();
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

    public void Ability1()
    {
        //TODO
        Debug.Log("Ability1");
        abilityState = AbilityState.Idle;
    }

    public void Ability2()
    {
        //TODO
        Debug.Log("Ability2");
        abilityState = AbilityState.Idle;
    }

    public void Ability3()
    {
        //TODO
        Debug.Log("Ability3");
        abilityState = AbilityState.Idle;
    }


}
