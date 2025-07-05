using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using BezierSplineTool.Core;
using System.Collections.Generic;

namespace BezierSplineTool.Editor
{
    /// <summary>
    /// Enables to creating and manipulating SplineLane fully on editor.
    /// </summary>
    public class SplineEditor : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        private const float MinDistanceValue = 1.0f / 16;

        private Button m_CreateButton;
        private VisualElement m_DistanceModeContainer;
        private Button m_DistanceModeButton;
        private Toggle m_DistancePreviewToggle;
        private FloatField m_DistanceValue;

        private SplineLane m_SplineLane;

        [MenuItem("Window/BezierSplineTool/SplineEditor")]
        public static void ShowWindow()
        {
            SplineEditor wnd = GetWindow<SplineEditor>();
            wnd.titleContent = new GUIContent("SplineEditor");
            wnd.minSize = new Vector2(350, 250);
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            root.Add(m_VisualTreeAsset.Instantiate());
            SetupUIElements(root);
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void SetupUIElements(VisualElement root)
        {
            m_CreateButton = root.Q<Button>("create-button");
            m_DistanceModeButton = root.Q<Button>("distance-mode-button");
            m_DistanceValue = root.Q<FloatField>("value-input-field");
            m_DistanceModeContainer = root.Q<VisualElement>("distance-mode-container");
            m_DistancePreviewToggle = root.Q<Toggle>("enable-distanced-preview-toggle");

            if (m_CreateButton != null) m_CreateButton.clicked += OnCreateButtonClicked;
            if (m_DistanceModeButton != null) m_DistanceModeButton.clicked += OnDistanceModeButtonClicked;

            if (m_DistanceValue != null)
            {
                m_DistanceValue.value = 2;
                m_DistanceValue.RegisterValueChangedCallback(OnDistanceValueChanged);
            }

            if (m_DistancePreviewToggle != null)
            {
                m_DistancePreviewToggle.value = false;
                m_DistancePreviewToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_SplineLane != null)
                    {
                        m_SplineLane.SplineParameterizerL.GenerateDistributedPoints(m_DistanceValue.value);
                        m_SplineLane.SplineParameterizerR.GenerateDistributedPoints(m_DistanceValue.value);
                    }
                    SceneView.RepaintAll();
                });
            }

            if (m_DistanceModeContainer != null) m_DistanceModeContainer.style.display = DisplayStyle.None;
        }

        private void OnCreateButtonClicked()
        {
            m_CreateButton.style.display = DisplayStyle.None;
            m_DistanceModeContainer.style.display = DisplayStyle.Flex;
            m_SplineLane = new SplineLane(Vector3.zero);
            FocusCamera(Vector3.zero);

        }

        private void OnDistanceModeButtonClicked()
        {
            if (m_SplineLane == null) return;

            GenerateAndVisualizeDistributedPoints(m_SplineLane.SplineParameterizerL, m_SplineLane.SplineParameterizerR);
        }

        private void GenerateAndVisualizeDistributedPoints(params SplineParameterizer[] parameterizers)
        {
            GameObject spherePrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            foreach (var parameterizer in parameterizers)
            {
                parameterizer.GenerateDistributedPoints(m_DistanceValue.value);
                foreach (Vector3 position in parameterizer.DistributedPoints)
                {
                    Instantiate(spherePrefab, position, Quaternion.identity);
                }
            }

            DestroyImmediate(spherePrefab);
        }

        private void OnDistanceValueChanged(ChangeEvent<float> evt)
        {
            float newDistance = Mathf.Max(evt.newValue, MinDistanceValue);
            m_DistanceValue.SetValueWithoutNotify(newDistance);

            if (m_DistancePreviewToggle.value && m_SplineLane != null)
            {
                m_SplineLane.SplineParameterizerL.GenerateDistributedPoints(newDistance);
                m_SplineLane.SplineParameterizerR.GenerateDistributedPoints(newDistance);
                SceneView.RepaintAll();
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (m_SplineLane == null) return;

            DrawSplineHandles(m_SplineLane.SplineL, m_SplineLane.SplineParameterizerL);
            DrawSplineHandles(m_SplineLane.SplineR, m_SplineLane.SplineParameterizerR);

            DrawSpline(m_SplineLane.SplineParameterizerL, Color.green);
            DrawSpline(m_SplineLane.SplineParameterizerR, Color.blue);

            if (m_DistancePreviewToggle.value)
            {
                DrawDistributedPoints(m_SplineLane.SplineParameterizerL.DistributedPoints);
                DrawDistributedPoints(m_SplineLane.SplineParameterizerR.DistributedPoints);
            }
        }

        private void DrawSplineHandles(ISpline spline, SplineParameterizer parameterizer)
        {
            Vector3[] controlPoints = new Vector3[spline.ControlPoints.Length];
            for (int i = 0; i < controlPoints.Length; i++)
            {
                controlPoints[i] = spline.ControlPoints[i];
            }

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < controlPoints.Length; i++)
            {
                controlPoints[i] = Handles.PositionHandle(controlPoints[i], Quaternion.identity);
            }

            if (EditorGUI.EndChangeCheck())
            {

                spline.UpdateControlPoints(controlPoints);
                parameterizer.ParameterizeSpline();
                if (m_DistancePreviewToggle.value)
                {
                    parameterizer.GenerateDistributedPoints(m_DistanceValue.value);
                }
            }
        }

        private void DrawSpline(SplineParameterizer parameterizer, Color color)
        {
            Handles.color = color;
            Handles.DrawPolyLine(parameterizer.Points);
        }

        private void DrawDistributedPoints(IEnumerable<Vector3> points)
        {
            Handles.color = Color.white;
            foreach (Vector3 position in points)
            {
                Handles.SphereHandleCap(0, position, Quaternion.identity, 0.25f, EventType.Repaint);
            }
        }

        private void FocusCamera(Vector3 t_Origin)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                
                Quaternion cameraRotation = Quaternion.LookRotation(new Vector3(0.5f, -0.5f, 0.5f));

                
                sceneView.LookAt(t_Origin, cameraRotation, 10f);
                sceneView.Frame(new Bounds(t_Origin, Vector3.one*5), false);
            }
        }
    }
}