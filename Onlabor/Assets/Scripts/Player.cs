using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Player : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;
    private bool isSelected;
    private GameObject selectedCircle;

    public bool IsSelected{
        set { isSelected = value;
            selectedCircle.SetActive(value);
        }
        get { return isSelected; }
        }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        selectedCircle = transform.Find("Selected").gameObject;
        SetSelected(false);
    }

    // Update is called once per frame
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
