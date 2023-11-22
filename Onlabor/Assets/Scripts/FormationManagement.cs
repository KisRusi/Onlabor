using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onlabor
{
    
    public class FormationManagement : MonoBehaviour
    {
        public static FormationManagement Instance { get; private set; }
        private void Start()
        {
            Instance = this;
        }
        public List<Vector3> GetPositionList(Vector3 startPosition, float distance, int positionCount)
        {
            List<Vector3> positionList = new List<Vector3>();
            for (int i = 0; i < positionCount; i++)
            {
                float angle = i * (360f / positionCount);
                Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
                Vector3 position = startPosition + dir * distance;
                positionList.Add(position);
            }
            return positionList;
        }

        private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
        {
            return Quaternion.Euler(0, 0, angle) * vec;
        }


        public List<Vector3> _GetPositionList(int amountUnits)
        {
            Debug.Log("GetPositionAmounofunits:" + amountUnits);
            float anglePerUnit = (Mathf.PI * 2f) / amountUnits;
            float radius = amountUnits * (1.5f / 5f);
            List<Vector3> positionList = new List<Vector3>();
            for (int i = 0; i < amountUnits; i++)
            {
                Vector3 position = new Vector3(Mathf.Cos(anglePerUnit * i), 0f, Mathf.Sin(anglePerUnit * i)) * radius;
                positionList.Add(position);
            }
            return positionList;
        }
    }
}