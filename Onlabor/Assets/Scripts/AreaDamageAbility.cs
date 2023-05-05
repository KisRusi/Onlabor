using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamageAbility : MonoBehaviour
{
    [SerializeField]
    private Player player;
    private float maxCoolDown;
    private float coolDownTime;
    private bool coolDownChanged;

    

    private void Start()
    {
        maxCoolDown = 5f;
        coolDownTime = 0f;
        player.OnAreaDamageCoolDownChange += Player_OnCoolDownChange;
    }

    //Csak akkor megy az update ha active
    private void Update()
    {
        Debug.Log(coolDownChanged);
        if(coolDownChanged)
        {
            if(coolDownTime > 0)
            {
                Debug.Log(coolDownTime);
                coolDownTime -= Time.deltaTime;
            }
        }
        
    }

    private void Player_OnCoolDownChange(object sender, EventArgs e)
    {
        coolDownTime = maxCoolDown;
        coolDownChanged = true;
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



}
