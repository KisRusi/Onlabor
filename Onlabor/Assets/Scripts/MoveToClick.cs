using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MoveToClick : MonoBehaviour
{
    

    public static  Vector3 GetMousePosition()
    {
        Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(movePosition, out RaycastHit hitInfo))
        {
            return hitInfo.point;
        }else
        { 
            return Vector3.zero; 
        }
    }


    
}
