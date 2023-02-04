using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("줌")] [SerializeField] private float minZoomSize;
    [SerializeField] private float maxZoomSize;
    [SerializeField] private float zoomSpeed;

    [Header("드래그")] [SerializeField] private float dragSpeed;

    [Header("포커싱")] [SerializeField] private float focusSpeed;

    private Camera camera;
    private Vector2 touchStart;
    private Vector2 cameraStart;

    private float cameraHeight = -10;
    
    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        ProcessZoom();
        ProcessDrag();
    }

    private void ProcessZoom()
    {
        var delta = Input.mouseScrollDelta.y * zoomSpeed;

        cameraHeight = Mathf.Clamp(cameraHeight + delta, minZoomSize, maxZoomSize);
    }

    private void ProcessDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
            cameraStart = transform.position;
        }
        if (Input.GetMouseButton(0))
        {
            var touchPos = (Vector2)Input.mousePosition;

           // var speed = dragSpeed * Mathf.Abs(cameraHeight);
            
            Vector3 position = cameraStart;
            position += (Vector3)(touchStart - touchPos) * dragSpeed;
            position.z = cameraHeight;
            
            transform.position = position;
        }
        
    }
}