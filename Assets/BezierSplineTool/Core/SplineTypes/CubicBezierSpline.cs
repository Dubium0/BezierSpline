using System;
using UnityEngine;

namespace BezierSplineTool.Core
{
    

public class CubicBezierSpline : ISpline
    {
        private Vector3[] m_ControlPoints;

        // I precompute the coefficents than calculate the bezier curve with power series polynomial version of it.
        private Vector3[] m_PreComputedCoeffs;

        public event Action<ISpline> OnValidation;

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
                throw new Exception("Cubic Bezier Requires 4 control points!");
            }
            for (int i = 0; i < 4; i++)
            {
                m_ControlPoints[i] = t_ControlPoints[i];
            }
            
            PrecomputeCoeffs();
            OnValidation.Invoke(this);
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