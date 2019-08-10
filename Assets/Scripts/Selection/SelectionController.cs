using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public static SelectionController Instance { get; private set; }

    public List<Employee> SelectedEmployees { get; } = new List<Employee>();

    private GUIStyle selectionBoxStyle;
    
    private void Awake()
    {
        Instance = this;
        
        // Create transparent white texture
        var texture = new Texture2D(2, 2);
        var colors = new Color[4];
        for (int i = 0; i < colors.Length; ++i)
        {
            colors[i] = new Color(255, 255, 255, 0.4f);
        }
        texture.SetPixels(colors);
        texture.Apply();
            
        selectionBoxStyle = new GUIStyle();
        selectionBoxStyle.normal.background = texture;
    }

    private Vector3? startMousePos;
    private Vector3? currentMousePos;
    
//    private void Update()
//    {
//        if (Input.GetMouseButton(0))
//        {
//            if (startMousePos == null)
//            {
//                startMousePos = Input.mousePosition;
//            }
//
//            currentMousePos = Input.mousePosition;
//        }
//        else
//        {
//            startMousePos = null;
//        }
//    }

    private void OnGUI()
    {
        if (Input.GetMouseButton(0))
        {
            if (startMousePos == null)
            {
                startMousePos = Event.current.mousePosition;
            }

            currentMousePos = Event.current.mousePosition;
            
            float width = currentMousePos.Value.x - startMousePos.Value.x;
            float height = currentMousePos.Value.y - startMousePos.Value.y;
            var rect = new Rect(startMousePos.Value.x, startMousePos.Value.y, width, height);
            
            GUI.Box(rect, string.Empty, selectionBoxStyle);
        }
        else
        {
            startMousePos = null;
        }
    }

    public void OnEmployeeClicked(Employee employee)
    {
        if (SelectedEmployees.Contains(employee))
        {
            DeselectEmployee(employee);
        }
        else
        {
            SelectedEmployees.Clear();
            SelectEmployee(employee);
        }
    }

    private void SelectEmployee(Employee employee)
    {
        SelectedEmployees.Add(employee);
        employee.SetSelected(true);
    }

    private void DeselectEmployee(Employee employee)
    {
        SelectedEmployees.Remove(employee);
        employee.SetSelected(false);
    }
}
