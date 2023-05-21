using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircleUITurret : MonoBehaviour
{
    
    [SerializeField]
    private Turret turret;

    private float coolDown;
    private float readyTime;

    [SerializeField]
    private Image image;
    private float time;

    
    private void Awake()
    {
        turret = GetComponentInParent<Turret>();
        coolDown = turret.GetCoolDown();
        image.fillAmount = 0;
    }

    private void Update()
    {
        
        if(turret.GetState() == Turret.State.Building)
        {
        time += Time.deltaTime;
        if(time >= coolDown)
        {
            turret.SwitchState(Turret.State.Ready);
            gameObject.SetActive(false);
        }
        image.fillAmount = time / coolDown;

        }
        
    }

    public void SetReadyTime(float time)
    {
        readyTime = time + coolDown;
    }
}
