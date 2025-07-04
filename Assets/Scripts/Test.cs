
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
    
    private void PrecomputeCoeffs()
    {
        m_PreComputedCoeffs[0] = CubicSplineControlPoints[3] - 3.0f * CubicSplineControlPoints[2] + 3.0f * CubicSplineControlPoints[1] - CubicSplineControlPoints[0];
        m_PreComputedCoeffs[1] = 3.0f * (CubicSplineControlPoints[2] - 2.0f * CubicSplineControlPoints[1] + CubicSplineControlPoints[0]);
        m_PreComputedCoeffs[2] = 3.0f * (CubicSplineControlPoints[1] - CubicSplineControlPoints[0]);
        m_PreComputedCoeffs[3] = CubicSplineControlPoints[0];
    }

    private void FillDrawingPoints() {

        float step = 1.0f / (m_MaxDrawingResolution-1);
        for (int i = 0; i < m_MaxDrawingResolution; i++)
        {
            DrawingPoints[i] = GetPoint(i * step);
        }
    }

    public void UpdateSpline()
    {
        PrecomputeCoeffs();
        FillDrawingPoints();
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

    
}
