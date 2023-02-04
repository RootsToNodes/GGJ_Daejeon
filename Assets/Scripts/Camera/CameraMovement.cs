using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float borderSpace;
    
    [Header("줌")] [SerializeField] private float minZoomSize;
    [SerializeField] private float maxZoomSize;
    [SerializeField] private float zoomSpeed;

    [Header("드래그")] [SerializeField] private float dragSpeed;

    [Header("포커싱")] [SerializeField] private float focusSpeed;
    [SerializeField] private AnimationCurve focusAnimCurve;

    private Camera camera;
    private Vector2 touchStart;
    private Vector2 cameraStart;

    private float cameraHeight = -10;
    private bool canMove = true;

    private Rect border;
    private float heightCorrection;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        heightCorrection = Mathf.Sin(transform.rotation.eulerAngles.x * Mathf.Deg2Rad);
    }

    private void Update()
    {
        if (!canMove)
        {
            return;
        }

        ProcessZoom();
        ProcessDrag();
    }

    private void ProcessZoom()
    {
        var delta = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        cameraHeight = Mathf.Clamp(cameraHeight + delta, minZoomSize, maxZoomSize);
    }

    private void ProcessDrag()
    {
        Vector3 position = transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
            cameraStart = transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            var touchPos = (Vector2) Input.mousePosition;

            var speed = dragSpeed * Mathf.Abs(cameraHeight);

            position = cameraStart;
            position += (Vector3) (touchStart - touchPos) * speed;

            transform.position = position;
        }

        float correction = heightCorrection * Mathf.Abs(cameraHeight);

        position.x = Mathf.Clamp(position.x, border.xMin - borderSpace, border.xMax + borderSpace);
        position.y = Mathf.Clamp(position.y, border.yMin - borderSpace + correction, border.yMax + borderSpace + correction);
        position.z = cameraHeight;

        transform.position = position;
    }

    public void SetBorder(Rect border)
    {
        this.border = border;
    }

    public void FocusToTarget(Vector3 target)
    {
        target.z = cameraHeight;
        target.y += heightCorrection * Mathf.Abs(cameraHeight);

        StopAllCoroutines();
        StartCoroutine(focusProcess(target));
    }

    private IEnumerator focusProcess(Vector3 targetPos)
    {
        canMove = false;

        var startPos = transform.position;

        float progress = 0;
        while (progress <= 1)
        {
            progress += Time.deltaTime * focusSpeed;

            transform.position = Vector3.Lerp(startPos, targetPos, focusAnimCurve.Evaluate(progress));

            yield return null;
        }

        canMove = true;
    }
}