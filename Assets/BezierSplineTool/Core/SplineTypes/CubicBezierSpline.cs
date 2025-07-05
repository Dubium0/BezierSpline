using System;
using UnityEngine;

namespace BezierSplineTool.Core
{

    /// <summary>
    /// Bezier curve with 4 control points. It precomputes coefficients to achieve faster point access.
    /// </summary>
    public class CubicBezierSpline : ISpline
    {
        private Vector3[] m_ControlPoints = new Vector3[4];

        // I precompute the coefficents than calculate the bezier curve with power series polynomial version of it.
        private Vector3[] m_PreComputedCoeffs =new Vector3[4];

        public ReadOnlySpan<Vector3> ControlPoints => m_ControlPoints;


        public Vector3 GetPoint(float t)
        {
            float t_square = t * t;
            float t_cube = t_square * t;

            return m_PreComputedCoeffs[0] * t_cube + m_PreComputedCoeffs[1] * t_square + m_PreComputedCoeffs[2] * t + m_PreComputedCoeffs[3]; 
        }



        public void UpdateControlPoints(ReadOnlySpan<Vector3> t_ControlPoints)
        {
            if (t_ControlPoints.Length != 4)
            {
                throw new ArgumentException("Cubic Bezier Requires 4 control points!");
            }
            for (int i = 0; i < 4; i++)
            {
                m_ControlPoints[i] = t_ControlPoints[i];
            }

            PrecomputeCoeffs();
        
        }

        private void PrecomputeCoeffs()
        {
            m_PreComputedCoeffs[0] = m_ControlPoints[3] - 3.0f * m_ControlPoints[2] + 3.0f * m_ControlPoints[1] - m_ControlPoints[0];
            m_PreComputedCoeffs[1] = 3.0f * (m_ControlPoints[2] - 2.0f * m_ControlPoints[1] + m_ControlPoints[0]);
            m_PreComputedCoeffs[2] = 3.0f * (m_ControlPoints[1] - m_ControlPoints[0]);
            m_PreComputedCoeffs[3] = m_ControlPoints[0];
        }
    }
}