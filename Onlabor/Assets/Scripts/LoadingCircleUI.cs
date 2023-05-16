using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircleUI : MonoBehaviour
{
    [SerializeField]
    private BuildingManager buildingManager;

    private float coolDown;

    [SerializeField]
    private Image image;

    private void Start()
    {
        buildingManager.OnStoragePlaced += OnStoragePlaced;
        Debug.Log("circle start");
    }
    private void Awake()
    {
        coolDown = gameObject.GetComponentInParent<Canvas>().gameObject.GetComponentInParent<ResourceStorage>().GetCoolDown();
    }

    private void Update()
    {
        
    }

    private void OnStoragePlaced(object sender, BuildingManager.OnStoragePlaceEventArgs e)
    {
        Debug.Log("event fire");
        UpdateFillCircle();
    }

    public void UpdateFillCircle()
    {
        Debug.Log("fillcircle");
        var time = Time.time;
        var coolDownEnd = time + coolDown;
        while(time /coolDownEnd < 1)
        {
            image.fillAmount = time / coolDownEnd;
        }
        gameObject.SetActive(false);
    }
}
