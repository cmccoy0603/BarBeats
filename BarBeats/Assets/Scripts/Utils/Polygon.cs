using System;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    public LineRenderer polygonRenderer;

    private Color successColor;
    private Color failColor;

    public void Awake()
    {
        successColor = Color.green;
        successColor.a = .5f;
        failColor = Color.red;
        failColor.a = .5f;
    }

    public void DrawPolygon(int numSides, Vector2[] points, float rythymScore = 0)
    {
        polygonRenderer.positionCount = numSides;

        for (int i = 0; i < points.Length; i++)
        {
            polygonRenderer.SetPosition(i, points[i]);
        }

        if (rythymScore > 0)
        {
            polygonRenderer.startColor = successColor;
            polygonRenderer.endColor = successColor;
        }
        else
        {
            polygonRenderer.startColor = failColor;
            polygonRenderer.endColor = failColor;
        }

        polygonRenderer.loop = false;
    }
}
