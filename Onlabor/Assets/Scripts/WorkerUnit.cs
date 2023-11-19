using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerUnit : RtsUnit
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnemy)
        {
            ChangeHealthBarColor();

            switch (currentState)
            {
                case State.Normal:
                    break;
                case State.GoingTo_Gathering:
                    MoveToDestination(resourceNode.GetPosition());
                    float reachDestination = 2f;
                    if (Vector3.Distance(transform.position, resourceNode.GetPosition()) < reachDestination)
                    {
                        navMeshAgent.ResetPath();
                        currentState = State.Gathering;
                    }
                    break;
                case State.Gathering:
                    gatheringTime -= Time.deltaTime;
                    if (gatheringTime < 0)
                    {
                        float maxGatheringTime = 1f;
                        gatheringTime += maxGatheringTime;

                        resourceAmount++;
                        Debug.Log("Gather!" + resourceAmount);
                        int maxResourceAmount = 3;
                        if (resourceAmount > maxResourceAmount)
                        {
                            currentState = State.GoingBack_Gathering;
                        }
                    }
                    break;
                case State.GoingBack_Gathering:
                    CheckForResourceStorage();
                    if (resourceStorage == null)
                    {
                        currentState = State.WaitingForStorage;
                        goto case State.WaitingForStorage;
                    }
                    MoveToDestination(resourceStorage.transform.position);
                    reachDestination = 2f;
                    if (Vector3.Distance(transform.position, resourceStorage.transform.position) < reachDestination)
                    {

                        resourceAmount = 0;
                        currentState = State.GoingTo_Gathering;
                    }
                    break;
                case State.WaitingForStorage:
                    CheckForResourceStorage();
                    if (resourceStorage != null)
                    {
                        currentState = State.GoingBack_Gathering;
                    }
                    break;
            }
        }

    }
}
