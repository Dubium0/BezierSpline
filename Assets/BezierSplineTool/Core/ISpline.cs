

using System;
using System.Linq;
using UnityEngine;

namespace BezierSplineTool.Core
{


    public interface ISpline
    {
        Vector3 GetPoint(float t);
        void UpdateControlPoints(ReadOnlySpan<Vector3> t_ControlPoints);
        ReadOnlySpan<Vector3> ControlPoints { get; }
        
    }

    


}