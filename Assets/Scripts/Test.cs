
using System;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector3[] CubicSplineControlPoints = { -Vector3.right * 1.5f, -Vector3.right * .5f, Vector3.right * .5f, Vector3.right * 1.5f };

    // I precompute the coefficents than calculate the bezier curve with power series polynomial version of it.
    public Vector3[] m_PreComputedCoeffs = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    private const int m_MaxDrawingResolution = 32;

    public Vector3[] DrawingPoints = new Vector3[m_MaxDrawingResolution];

    public float[] DistanceLUT = new float[m_MaxDrawingResolution];

    public List<GameObject> DistancePoints = new();

    private void PrecomputeCoeffs()
    {
        m_PreComputedCoeffs[0] = CubicSplineControlPoints[3] - 3.0f * CubicSplineControlPoints[2] + 3.0f * CubicSplineControlPoints[1] - CubicSplineControlPoints[0];
        m_PreComputedCoeffs[1] = 3.0f * (CubicSplineControlPoints[2] - 2.0f * CubicSplineControlPoints[1] + CubicSplineControlPoints[0]);
        m_PreComputedCoeffs[2] = 3.0f * (CubicSplineControlPoints[1] - CubicSplineControlPoints[0]);
        m_PreComputedCoeffs[3] = CubicSplineControlPoints[0];
    }

    public float Distance = 1;

    private void FillDrawingPoints()
    {

        float step = 1.0f / (m_MaxDrawingResolution - 1);
        for (int i = 0; i < m_MaxDrawingResolution; i++)
        {
            DrawingPoints[i] = GetPoint(i * step);
        }
    }

    private void FillDistanceLUT()
    {
        DistanceLUT[0] = 0;
        for (int i = 1; i < m_MaxDrawingResolution; i++)
        {
            DistanceLUT[i] = DistanceLUT[i - 1] + Vector3.Distance(DrawingPoints[i - 1], DrawingPoints[i]);
        }
    }

    public float GetTFromDistanceLUT(float distance)
    {
        int tail = m_MaxDrawingResolution - 1;
        int head = 0;
        int middle = tail / 2;
        float step = 1.0f / (m_MaxDrawingResolution - 1);
        if (distance >= DistanceLUT[m_MaxDrawingResolution - 1])
        {
            return 1; // this should throw error later
        }
        else if (distance <= 0)
        {
            return 0;
        }

        while (true)
        {
            if (distance == DistanceLUT[middle])
            {
                return middle * step;
            }
            if (middle == head || middle == tail)
            {
                break;
            }

            if (distance > DistanceLUT[middle])
            {
                head = middle;
                middle = (head + tail) / 2;
            }
            else
            {
                tail = middle;
                middle = (head + tail) / 2;
            }

        }
        float interpolationFactor = (distance - DistanceLUT[head]) / (DistanceLUT[tail] - DistanceLUT[head]);
        float upperBound = tail * step;
        float lowerBound = head * step;
        return (upperBound - lowerBound) * interpolationFactor + lowerBound;

    }
    public void UpdateSpline()
    {
        PrecomputeCoeffs();
        FillDrawingPoints();
        FillDistanceLUT();
    }

    public Vector3 GetPoint(float t)
    {
        float t_square = t * t;
        float t_cube = t_square * t;

        return m_PreComputedCoeffs[0] * t_cube + m_PreComputedCoeffs[1] * t_square + m_PreComputedCoeffs[2] * t + m_PreComputedCoeffs[3];
    }

    public Vector3 GetVelocity(float t)
    {
        float t_square = t * t;

        return m_PreComputedCoeffs[0] * 3 * t_square + m_PreComputedCoeffs[1] * 2 * t + m_PreComputedCoeffs[2];
    }

    public void DistributeDistancePoints(float distance)
    {
        if (distance <= 0)
        {
            throw new Exception("Distance cannot be zero");
        }
        DistancePoints.Clear();

        float maxDistance = DistanceLUT[m_MaxDrawingResolution - 1];

        if ((maxDistance / distance) > 128)
        {
            distance = maxDistance / 128.0f; // capping to 128 because it can grow a LOT.
        }

        for (float distanceToRequest = 0; distanceToRequest <= maxDistance; distanceToRequest += distance) // +1 because |-| if the - interval | corresponds to points and number of points always +1 more.
        {
            Vector3 spawnPoint = GetPoint(GetTFromDistanceLUT(distanceToRequest));
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = spawnPoint;
            DistancePoints.Add(sphere);

        }
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawLineStrip(DrawingPoints, false);
    }

}
