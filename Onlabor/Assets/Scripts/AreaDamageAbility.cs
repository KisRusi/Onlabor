using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class AreaDamageAbility : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private float coolDownTime;
    private float areaDamage;

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Button btn;

    private void Awake()
    {
        coolDownTime = 5;
        areaDamage = 65;
    }
    

    
    private void Update()
    {
        if(player.abilityState == AbilityState.Ability1)
        {
            
            var targetUnits = player.GetTargetUnits();
            gameObject.transform.position = GetMousePos();
            if(Input.GetMouseButtonDown(0) && targetUnits.Count > 0)
            {
                if(targetUnits.Count > 1)
                {
                    var dividedDamage = areaDamage / targetUnits.Count;
                    foreach(var unit in targetUnits)
                    {
                        unit.GetComponent<RtsUnit>().Damage(dividedDamage);
                    }
                    targetUnits.Clear();
                    player.ClearTargetUnits();
                }
                else
                {
                    targetUnits.FirstOrDefault().GetComponent<RtsUnit>().Damage(areaDamage);
                    targetUnits.Clear();
                    player.ClearTargetUnits();
                }
                gameObject.SetActive(false);
                player.abilityState = AbilityState.Idle;
                player.NextAreaDamageTime = Time.time + coolDownTime;
                btn.interactable = false; 
            }
        }
        
    }






    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Unit"))
        {
            if(other.gameObject.GetComponent<RtsUnit>().IsEnemy())
                player.AddTarget(other.gameObject);
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name.Contains("Unit"))
        {
            if (other.gameObject.GetComponent<RtsUnit>().IsEnemy())
                player.RemoveTarget(other.gameObject);
        }
    }

    public float GetCoolDown()
    {
        return coolDownTime;
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
