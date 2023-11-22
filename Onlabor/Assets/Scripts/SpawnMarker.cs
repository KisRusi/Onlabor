using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMarker : MonoBehaviour
{
    private float radius = 4f;
    private Vector3 parentPos;
    public bool IsActive
    {
        get;
        set;
    }

    [SerializeField]
    private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        parentPos = gameObject.GetComponentInParent<Transform>().transform.position;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            gameObject.transform.position = GetMousePos();
        }
        if(Input.GetMouseButtonUp(0) && IsActive == true)
        {
            if(gameObject.transform.position.x > (parentPos.x + radius))
            {
                gameObject.transform.position = new Vector3( parentPos.x + radius, parentPos.y, gameObject.transform.position.z );
            }
            if(gameObject.transform.position.z > (parentPos.z + radius)) 
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, parentPos.y, parentPos.z + radius);
            }
            if (gameObject.transform.position.x < (parentPos.x - radius))
            {
                gameObject.transform.position = new Vector3(parentPos.x - radius, parentPos.y, gameObject.transform.position.z);
            }
            if (gameObject.transform.position.z < (parentPos.z - radius))
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, parentPos.y, parentPos.z - radius);
            }
            SetActive(false);
        }

        if(RTSMain.Instance.GetSelectedBarrack() == null)
        {
            gameObject.SetActive(false);
        }
    }


    public Vector3 GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 position;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            position = hit.point;
            position.y = 0.01f;
            return position;
        }
        return Vector3.zero;
    }

    public void SetActive(bool value)
    {
        this.IsActive = value;
    }
}
