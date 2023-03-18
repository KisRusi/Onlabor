using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Player : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;
    private bool isSelected;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void MoveToDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
    }

    public void setSelected(bool select)
    {

    }
}
