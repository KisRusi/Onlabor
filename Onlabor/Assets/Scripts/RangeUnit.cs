using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUnit : RtsUnit
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnemy)
        {
            ChangeHealthBarColor();

            switch(currentState)
            {
                case State.Normal:
                    AutomaticAttackInArea(transform.position, 8f, 6f);
                    break;
                case State.MoveToTarget:
                    if(targetUnit.IsDead())
                    {
                        MoveAndResetState(GetPosition());
                    }
                    else
                    {
                        MoveToDestination(targetUnit.GetPosition());
                        float reachDestination = 6f;
                        if (Vector3.Distance(transform.position, targetUnit.transform.position) < reachDestination)
                        {
                            currentState = State.Attacking;
                        }
                    }
                    break;
                case State.Attacking:
                    if (enemies.Count == 0)
                        MoveAndResetState(GetPosition());
                    attackTime -= Time.deltaTime;
                    float attackTimerMax = 1f;
                    if (targetUnit.IsDead())
                    {
                        AutomaticAttackInArea(transform.position, 8f, 6f);
                    }
                    if (attackTime < 0)
                    {
                        attackTime += attackTimerMax;
                        targetUnit.Damage(20);
                    }
                    break;
            }
        }
    }

    
}
