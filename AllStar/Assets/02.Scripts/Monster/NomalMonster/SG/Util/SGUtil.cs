using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SGUtil
{
    public static readonly Vector3 VECTOR3_ZERO = Vector3.zero;
    public static readonly Vector3 VECTOR3_ONE = Vector3.one;
    public static readonly Vector3 VECTOR3_HALF = new Vector3(0.5f, 0.5f, 0.5f);

    public static readonly Vector2 VECTOR2_ZERO = Vector2.zero;
    public static readonly Vector2 VECTOR2_ONE = Vector2.one;
    public static readonly Vector2 VECTOR2_HALF = new Vector2(0.5f, 0.5f);

    public static readonly Quaternion QUATERNION_IDENTITY = Quaternion.identity;
    public enum AXIS
    {
        X_AND_Y,
        X_AND_Z,
    }

    public enum TIME
    {
        DELTA_TIME,
        UNSCALED_DELTA_TIME,
        FIXED_DELTA_TIME,
    }


    public static float GetAngleFromTwoPosition(Transform fromTrans, Transform toTrans, AXIS axisMove)
    {
        switch (axisMove)
        {
            case AXIS.X_AND_Y:
                return GetZangleFromTwoPosition(fromTrans, toTrans);
            case AXIS.X_AND_Z:
                return GetYangleFromTwoPosition(fromTrans, toTrans);
            default:
                return 0f;
        }


    }

    private static float GetZangleFromTwoPosition(Transform fromTrans, Transform toTrans)
    {
        if (fromTrans == null || toTrans == null)
        {
            return 0f;
        }
        float xDistance = toTrans.position.x - fromTrans.position.x;
        float yDistance = toTrans.position.y - fromTrans.position.y;
        float angle = (Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg) - 90f;
        angle = GetNormalizedAngle(angle);

        return angle;
    }

    private static float GetYangleFromTwoPosition(Transform fromTrans, Transform toTrans)
    {
        if (fromTrans == null || toTrans == null)
        {
            return 0f;
        }
        float xDistance = toTrans.position.x - fromTrans.position.x;
        float zDistance = toTrans.position.z - fromTrans.position.z;
        float angle = (Mathf.Atan2(zDistance, xDistance) * Mathf.Rad2Deg) - 90f;
        angle = GetNormalizedAngle(angle);

        return angle;
    }

    public static float GetNormalizedAngle(float angle)
    {
        while (angle < 0f)
        {
            angle += 360f;
        }
        while (360f < angle)
        {
            angle -= 360f;
        }
        return angle;
    }
    public static float GetShiftedAngle(int wayIndex, float baseAngle, float betweenAngle)
    {
        float angle = wayIndex % 2 == 0 ?
            baseAngle - (betweenAngle * (float)wayIndex / 2f) :
            baseAngle + (betweenAngle * Mathf.Ceil((float)wayIndex / 2f));

        return angle;
    }
}



