using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mouse
{
    public static Vector3 GetWorldPosition(float planeHeight)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = GetCameraDistanceToHeight(planeHeight);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return mousePosition;
    }

    private static float GetCameraDistanceToHeight(float planeHeight)
    {
        float distance = planeHeight;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up * distance);
        plane.Raycast(ray, out distance);
        return distance;
    }
}
