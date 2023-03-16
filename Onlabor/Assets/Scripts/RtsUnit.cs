using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsUnit : MonoBehaviour
{
    private GameObject unit;
    private void Awake()
    {
        unit = GetComponent<GameObject>();
    }
}
