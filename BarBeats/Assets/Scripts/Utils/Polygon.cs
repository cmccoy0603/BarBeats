using UnityEngine;

public class Polygon : MonoBehaviour
{
    public LineRenderer polygonRenderer;

    public void DrawPolygon(int numSides, Vector2[] points, float rotation = 0, float rythymScore = 0)
    {
        polygonRenderer.positionCount = numSides;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 rotatedPoint = RotatePoint(points[i], Vector2.zero, rotation);
            polygonRenderer.SetPosition(i, rotatedPoint);
        }

        if (rythymScore >= .8)
        {
            polygonRenderer.startColor = Color.green;
            polygonRenderer.endColor = Color.green;
        }
        else
        {
            polygonRenderer.startColor = Color.red;
            polygonRenderer.endColor = Color.red;
        }

        polygonRenderer.loop = false;
    }
    
    private Vector3 RotatePoint(Vector2 point, Vector2 pivot, float radians)
    {
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        Vector2 dir = point - pivot;

        float rotatedX = dir.x * cos - dir.y * sin;
        float rotatedY = dir.x * sin + dir.y * cos;

        return new Vector3(pivot.x + rotatedX, pivot.y + rotatedY, -1f);
    }
}
