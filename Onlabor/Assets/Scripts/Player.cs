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
    private GameObject areaDamageCircle;
    private HealthSystem healthSystem;
    private AbilityState abilityState;

    [SerializeField]
    private LayerMask layerMask;

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
        areaDamageCircle = transform.Find("AreaDamageCircle").gameObject;
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
        
        areaDamageCircle.SetActive(true);
        areaDamageCircle.transform.position = GetMousePos();
        if(Input.GetMouseButtonDown(0))
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

    public Vector3 GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 position;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            position = hit.point;
            position.y = 0.01f;
            return position;
        }
        return Vector3.zero;
    }



}
