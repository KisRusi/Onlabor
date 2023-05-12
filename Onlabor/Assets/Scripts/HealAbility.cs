using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class HealAbility : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private float coolDownTime;
    private float healAmount;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Button btn;

    private void Awake()
    {
        coolDownTime = 8;
        healAmount = 30;
    }

    private void Update()
    {
        if(player.abilityState == AbilityState.Ability2)
        {
            var targetUnits = player.GetTargetUnits();
            gameObject.transform.position = GetMousePos();
            if(Input.GetMouseButtonDown(0) /*&& targetUnits.Count > 0*/ )
            {
                foreach(var unit in targetUnits)
                {
                    unit.GetComponent<RtsUnit>().Heal(healAmount);
                }
                targetUnits.Clear();
                player.ClearTargetUnits();
                if (player.IsInHealArea)
                    player.Heal(healAmount);
                player.IsInHealArea = false;
                gameObject.SetActive(false);
                player.abilityState = AbilityState.Idle;
                player.NextHealTime = Time.time + coolDownTime;
                btn.interactable = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Unit"))
        {
            if (other.gameObject.GetComponent<RtsUnit>())
                player.AddTarget(other.gameObject);
        }
        if(other.gameObject.name.Contains("Player"))
            player.IsInHealArea = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Unit"))
        {
            if (other.gameObject.GetComponent<RtsUnit>())
                player.RemoveTarget(other.gameObject);
        }
        if (other.gameObject.name.Contains("Player"))
            player.IsInHealArea = false;
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
