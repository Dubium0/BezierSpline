
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Test)), CanEditMultipleObjects]
public class TestEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        Test example = (Test)target;

        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < 4; i++)
        {
            example.CubicSplineControlPoints[i] = Handles.PositionHandle(example.CubicSplineControlPoints[i], Quaternion.identity);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(example, "Change Look At Target Position");
            example.UpdateSpline();
        }
        Handles.color = Color.blue;
        Handles.DrawPolyLine(example.DrawingPoints);
    }
    
    
    
    
}