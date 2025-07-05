# Bezier Spline Tool

This tool allows you to create and manipulate Bezier splines within the Unity editor. It provides a visual interface for adjusting control points and generating points along the spline at a specified distance.

## Features

*   **Create and Edit Bezier Splines:** Easily create and modify cubic Bezier splines.
*   **Distance-based Point Generation:** Generate points along the spline at a specified distance.
*   **Visual Feedback:** See the spline and its control points in the scene view.
*   **Custom Editor Window:** A dedicated editor window for managing your splines.

## How to Use

1.  **Open the Spline Editor:** Go to `Window > BezierSplineTool > SplineEditor`.
2.  **Create a Spline:** Click the "Create" button in the editor window. This will create a new spline in the scene.
3.  **Manipulate the Spline:** Use the handles in the scene view to move the control points of the spline.
4.  **Generate Points:**
    *   Enable the "Distance Preview" toggle to see a preview of the generated points.
    *   Adjust the "Distance" value to control the spacing of the points.
    *   Click the "Generate Points" button to create spheres at the generated points.

## Code Structure

The project is organized into two main folders:

*   **Core:** Contains the core logic for the Bezier spline, including:
    *   `ISpline.cs`: An interface for splines.
    *   `CubicBezierSpline.cs`: An implementation of a cubic Bezier spline.
    *   `SplineLane.cs`: Manages two splines to create a lane.
    *   `SplineParameterizer.cs`:  Handles the generation of points along the spline.
*   **Editor:** Contains the editor-specific code, including:
    *   `SplineEditor.cs`: The main editor window for the tool.
    *   `SplineEditor.uxml`: The UI layout for the editor window.
    *   `SplineEditor.uss`: The stylesheet for the editor window.

## Future Improvements

*   Add support for different spline types.
*   Allow for more customization of the generated points.
*   Add the ability to save and load splines.