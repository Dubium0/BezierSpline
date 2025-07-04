
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Test)), CanEditMultipleObjects]
public class TestEditor : Editor
{
    void OnEnable()
    {
        Test example = (Test)target;
        example.UpdateSpline();
    }
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
    
    public override void OnInspectorGUI()
    {
        Test example = (Test)target;

        DrawDefaultInspector();
        if (GUILayout.Button(new GUIContent("Get Distance", "nothing")))
        {
            float t = example.GetTFromDistanceLUT(example.Distance);
            Debug.Log(" Calculated t : " + t);
            Debug.Log(" Point at t : " + example.GetPoint(t));
        }
    }
    
    
    
}