using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{

    private float coolDown;

    private void Awake()
    {
        coolDown = 5f;
    }

    public float GetCoolDown()
    {
        return coolDown;
    }
}
