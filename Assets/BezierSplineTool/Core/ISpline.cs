

using System;
using System.Linq;
using UnityEngine;

namespace BezierSplineTool.Core
{

    /// <summary>
    /// Interface for any kind of Spline. For this case this is actually unnecessary but I wanted to provide a flexibility for future improvements.
    /// </summary>
    public interface ISpline
    {
        Vector3 GetPoint(float t);
        void UpdateControlPoints(ReadOnlySpan<Vector3> t_ControlPoints);
        ReadOnlySpan<Vector3> ControlPoints { get; }

        
    }

    


}