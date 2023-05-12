using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircleUI : MonoBehaviour
{
    [SerializeField]
    private BuildingManager buildingManager;

    [SerializeField]
    private Image image;

    private void Start()
    {
        buildingManager.OnStoragePlaced += OnStoragePlaced;
    }

    private void OnStoragePlaced(object sender, BuildingManager.OnStoragePlaceEventArgs e)
    {
        UpdateFillCircle();
    }
}
