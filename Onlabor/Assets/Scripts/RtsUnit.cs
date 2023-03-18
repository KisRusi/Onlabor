using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RtsUnit : MonoBehaviour
{
    private bool isSelected;
    private GameObject selectedCircle;

    private NavMeshAgent navMeshAgent;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        selectedCircle = transform.Find("Selected").gameObject;
        SetSelected(false);
    }
    private void Update()
    {
        
    }

    public void MoveToDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
    }

    public void SetSelected(bool isSelected)
    {
        selectedCircle.SetActive(isSelected);
    }
}
