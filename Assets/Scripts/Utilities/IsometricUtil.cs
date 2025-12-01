using UnityEngine;

public static class IsometricUtil
{
    public static Vector3 CartesianToIsometric(Vector3 cartesian)
    {
        Vector3 screenPos = new Vector3(0, 0, 0)
        {
            x = cartesian.x - cartesian.y, 
            y = (cartesian.x + cartesian.y) / 2
        };
        return screenPos;
    }
}
