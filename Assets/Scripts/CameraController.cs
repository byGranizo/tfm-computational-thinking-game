using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private readonly float planeHeight = 0;

    private Vector3 mouseOrigin;
    private bool isDragging = false;

    [SerializeField]
    private int maxZoom = 10;
    [SerializeField]
    private int minZoom = 1;
    [SerializeField]
    private float zoomScale = 1.0f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (!gameManager.IsCameraMovementAllowed())
        {
            isDragging = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseOrigin = Mouse.GetWorldPosition(planeHeight);
            isDragging = true;
        }

        isDragging = Input.GetMouseButton(0);

        if (isDragging)
        {
            Vector3 mousePositionIncrement = Mouse.GetWorldPosition(planeHeight) - mouseOrigin;

            Vector3 cameraTranslate = -mousePositionIncrement;
            transform.Translate(new Vector3(cameraTranslate.x, 0, cameraTranslate.z), Space.World);

            mouseOrigin = Mouse.GetWorldPosition(planeHeight);
        }

        float scroll = Input.mouseScrollDelta.y;
        const float epsilon = 0.001f; // Tolerance level
        if (Math.Abs(scroll) > epsilon)
        {
            Camera mainCamera = Camera.main;
            float targetZoom = mainCamera.orthographicSize - scroll * zoomScale;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

            mainCamera.orthographicSize = targetZoom;
        }

    }
}
