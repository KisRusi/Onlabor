using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    private void Awake()
    {
        startHostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            SceneManager.LoadScene(1);
            Hide();
        });


        startClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            SceneManager.LoadScene(1);
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
