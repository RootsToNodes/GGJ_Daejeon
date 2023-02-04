using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private float cameraAnimSpeed;
    [SerializeField] private AnimationCurve cameraAnimCurve;

    [SerializeField] private float minCamsize;
    [SerializeField] private float border;
    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    public void UpdateMiniMapCamera(Tree tree)
    {
        Vector3 pos = tree.treeArea.center;
        pos.z = -10;
        
        var size = Mathf.Max(tree.treeArea.width, tree.treeArea.height) * 0.5f;
        
        StopAllCoroutines();
        StartCoroutine(MiniMapMove(pos,Mathf.Max(minCamsize, size + border)));
    }

    private IEnumerator MiniMapMove(Vector3 targetPos, float targetSize)
    {
        var startPos = transform.position;
        var startSize = camera.orthographicSize;

        float progress = 0;
        while (progress <= 1)
        {
            progress += Time.deltaTime * cameraAnimSpeed;

            transform.position = Vector3.Lerp(startPos, targetPos, cameraAnimCurve.Evaluate(progress));
            camera.orthographicSize = Mathf.Lerp(startSize, targetSize, cameraAnimCurve.Evaluate(progress));

            yield return null;
        }
    }
}