using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SplineEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private const float MinDistanceValue = 1.0f / 16;

    private Button m_CreateButton;
    private VisualElement m_DistanceModeContainer;
    private Button m_DistanceModeButton;
    private FloatField m_DistanceValue;


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

        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
        SetupUIElements(root);
    }


    private void SetupUIElements(VisualElement t_Root)
    {
        m_CreateButton = t_Root.Q<Button>("create-button");
        m_DistanceModeButton = t_Root.Q<Button>("distance-mode-button");
        m_DistanceValue = t_Root.Q<FloatField>("value-input-field");
        m_DistanceModeContainer = t_Root.Q<VisualElement>("distance-mode-container");

        if (m_CreateButton != null)
        {
            m_CreateButton.clicked += OnCreateButtonClicked;
        }

       
        if (m_DistanceModeButton != null)
        {
            m_DistanceModeButton.clicked += OnDistanceModeButtonClicked;

        }

       
        if (m_DistanceValue != null)
        {

            m_DistanceValue.value = MinDistanceValue;
            m_DistanceValue.RegisterValueChangedCallback(OnDistanceValueChanged);

        }
        if (m_DistanceModeContainer != null)
        {
            m_DistanceModeContainer.style.display = DisplayStyle.None;
        }
    }

    private void OnDistanceValueChanged(ChangeEvent<float> evt)
    {
        if (evt.newValue < MinDistanceValue)
        {
            m_DistanceValue.SetValueWithoutNotify(MinDistanceValue);
        }
    }

    private void OnDistanceModeButtonClicked()
    {
        Debug.Log("Distance Mode button clicked!");
    }

    private void OnCreateButtonClicked()
    {
       
        m_DistanceModeContainer.style.display = DisplayStyle.Flex;

        m_CreateButton.style.display = DisplayStyle.None;
    }
}
