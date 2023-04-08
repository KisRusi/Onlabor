using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{

    [SerializeField] private bool useEdgeScroll = false;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float FOVMax = 80;
    [SerializeField] private float FOVMin = 10;
    private float targetFOV = 50;

   private void Update()
    {

        HandleCameraMovement();

        if (useEdgeScroll)
        {
            HandleCameraMovement_EdgeScroll();
        }

        HandleCameraRotation();

        
        HandleCameraZoom();

    }

    private void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 20f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleCameraMovement_EdgeScroll()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        int edgeScrollSize = 20;

        if (Input.mousePosition.x < edgeScrollSize) inputDir.x -= 1f;
        if (Input.mousePosition.y < edgeScrollSize) inputDir.z -= 1f;
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x += 1f;
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z += 1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 20f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

    }
    private void HandleCameraRotation() 
    {
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateDir += 1f;
        if (Input.GetKey(KeyCode.E)) rotateDir -= 1f;

        float rotateSpeed = 100f;
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }

    private void HandleCameraZoom()
    {

        if(Input.mouseScrollDelta.y < 0) targetFOV += 5;
        if(Input.mouseScrollDelta.y > 0) targetFOV -= 5;

        targetFOV = Mathf.Clamp(targetFOV, FOVMin, FOVMax);
        float zoomSpeed = 10f;
        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
