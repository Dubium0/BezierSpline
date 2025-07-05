
using System;
using UnityEngine;

namespace BezierSplineTool.Core
{
    /// <summary>
    /// Generates the necessery data to visualize and control 2 bezier lines.
    /// </summary>
    public class SplineLane
    {
        private CubicBezierSpline m_SplineR;
        private CubicBezierSpline m_SplineL;

        private SplineParameterizer m_SplineParameterizerR;
        private SplineParameterizer m_SplineParameterizerL;


        public CubicBezierSpline SplineR => m_SplineR;
        public CubicBezierSpline SplineL => m_SplineL;

        public SplineParameterizer SplineParameterizerR => m_SplineParameterizerR;
        public SplineParameterizer SplineParameterizerL => m_SplineParameterizerL;

        // this is hard coded for simplicity it can be further parameterized
        public SplineLane(Vector3 t_Origin)
        {
            m_SplineR = new();
            m_SplineL = new();
            m_SplineParameterizerR = new(m_SplineR);
            m_SplineParameterizerL = new(m_SplineL);

            Vector3 offset = t_Origin + Vector3.right * 1;

            Vector3[] initialPositions = { Vector3.forward * -3 + offset, Vector3.forward * -1 + offset, Vector3.forward * 1 + offset, Vector3.forward * 3 + offset };

            m_SplineR.UpdateControlPoints(initialPositions);

            for (int i = 0; i < 4; i++)
            {
                initialPositions[i] = initialPositions[i] + offset * -2;
            }

            m_SplineL.UpdateControlPoints(initialPositions);

            m_SplineParameterizerR.ParameterizeSpline();
            m_SplineParameterizerL.ParameterizeSpline();
        }


        

        
        
    }


}