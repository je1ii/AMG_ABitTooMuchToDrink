using UnityEngine;

public static class Utils
{
    private const float IsometricSquashFactor = 0.5f; 
    
    public static Vector3 CartesianToIsometric(Vector3 cartesian)
    {
        Vector3 screenPos = new Vector3(0, 0, 0)
        {
            x = cartesian.x - cartesian.y, 
            y = (cartesian.x + cartesian.y) / 2
        };
        return screenPos;
    }
    
    public static float CalculateMetric(GameObject origin, Transform target, float range)
    {
        Vector3 a = origin.transform.position;
        Vector3 b = target.position; 

        float dx = b.x - a.x;
        float dy = b.y - a.y; 

        float rx = range;
        float ry = range * IsometricSquashFactor;

        float metric = (dx * dx) / (rx * rx) + (dy * dy) / (ry * ry);
        return metric;
    }
}
