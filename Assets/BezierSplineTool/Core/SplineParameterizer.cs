





using System;
using System.Collections.Generic;
using UnityEngine;

namespace BezierSplineTool.Core
{

    public class SplineParameterizer
    {

        private const int Resolution = 32;

        private float[] m_DistanceLUT = new float[Resolution];

        public float ArcLength => m_DistanceLUT[Resolution - 1];

        private Vector3[] m_Points = new Vector3[Resolution];

        public Vector3[] Points => m_Points;

        private List<Vector3> m_DistributedPoints = new();
        public List<Vector3> DistributedPoints => m_DistributedPoints;

        private readonly ISpline m_Spline;

        public SplineParameterizer(ISpline t_Spline) {
            m_Spline = t_Spline;
            
        }

        public void ParameterizeSpline()
        {
            FillPoints();
            //FillDistanceLUT(); I only need to Generate the lut before generating Distributed Points;
        }


        public float GetT(float distance)
        {
            int tail = Resolution - 1;
            int head = 0;
            int middle = tail / 2;
            float step = 1.0f / (Resolution - 1);
            if (distance >= m_DistanceLUT[Resolution - 1])
            {
                return 1; // this should throw error later
            }
            else if (distance <= 0)
            {
                return 0;
            }

            while (true)
            {
                if (distance == m_DistanceLUT[middle])
                {
                    return middle * step;
                }
                if (middle == head || middle == tail)
                {
                    break;
                }

                if (distance > m_DistanceLUT[middle])
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
            float interpolationFactor = (distance - m_DistanceLUT[head]) / (m_DistanceLUT[tail] - m_DistanceLUT[head]);
            float upperBound = tail * step;
            float lowerBound = head * step;
            return (upperBound - lowerBound) * interpolationFactor + lowerBound;

        }


        private void FillPoints()
        {
            float step = 1.0f / (Resolution - 1);
            for (int i = 0; i < Resolution; i++)
            {
                m_Points[i] = m_Spline.GetPoint(i * step);
            }
        }
        private void FillDistanceLUT()
        {

            for (int i = 1; i < Resolution; i++)
            {

                m_DistanceLUT[i] = m_DistanceLUT[i - 1] + Vector3.Distance(m_Points[i - 1], m_Points[i]);

            }
        }

        public void GenerateDistributedPoints(float t_Distance)
        {
            FillDistanceLUT();
            if (t_Distance <= 0)
            {
                throw new Exception("Distance cannot be negative or zero");
            }

            int numberOfPoints = (int)MathF.Round(ArcLength / t_Distance) + 1;
            m_DistributedPoints.Clear();
            for (int i = 0; i < numberOfPoints; i++)
            {
                m_DistributedPoints.Add(m_Spline.GetPoint(GetT(i * t_Distance)));
            }
            
        }

        
        
        
    }

}