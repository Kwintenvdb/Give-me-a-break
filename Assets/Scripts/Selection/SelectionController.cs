using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private float minSelectionRectSize = 10f;
    
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

            // Otherwise this might interfere with the click detection on individual employees
            if (Mathf.Abs(height) < minSelectionRectSize || Mathf.Abs(width) < minSelectionRectSize) return;
            
            var rect = new Rect(startMousePos.Value.x, startMousePos.Value.y, width, height);
            
            GUI.Box(rect, string.Empty, selectionBoxStyle);
            
            // Do this only after we "stop" selecting? E.g. continuously or just a single time?
            FindEmployeesInRect(rect);
        }
        else
        {
            startMousePos = null;
        }
    }

    private void FindEmployeesInRect(Rect rect)
    {
        var screenRect = GUIUtility.GUIToScreenRect(rect);
        var employees = FindObjectsOfType<Employee>();
        var mainCam = Camera.main;
        foreach (var employee in employees)
        {
            // Gotta flip the Y axis here to be in GUI space
            var employeeScreenPos = mainCam.WorldToScreenPoint(employee.transform.position);
            employeeScreenPos.y = Screen.height - employeeScreenPos.y;
            
            if (screenRect.Contains(employeeScreenPos))
            {
                SelectEmployee(employee);
            }
            else
            {
                DeselectEmployee(employee);
            }
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
        if (!SelectedEmployees.Contains(employee))
        {
            SelectedEmployees.Add(employee);
            employee.SetSelected(true);
        }
    }

    private void DeselectEmployee(Employee employee)
    {
        SelectedEmployees.Remove(employee);
        employee.SetSelected(false);
    }
}
