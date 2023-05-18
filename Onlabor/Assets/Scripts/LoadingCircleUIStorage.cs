using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircleUIStorage : MonoBehaviour
{
    
    [SerializeField]
    private ResourceStorage resourceStorage;

    private float coolDown;
    private float readyTime;

    [SerializeField]
    private Image image;
    private float time;

    
    private void Awake()
    {
        resourceStorage = GetComponentInParent<ResourceStorage>();
        coolDown = resourceStorage.GetCoolDown();
        image.fillAmount = 0;
    }

    private void Update()
    {
        
        if(resourceStorage.GetState() == ResourceStorage.State.Building)
        {
        time += Time.deltaTime;
        if(time >= coolDown)
        {
            resourceStorage.SwitchState(ResourceStorage.State.Ready);
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
