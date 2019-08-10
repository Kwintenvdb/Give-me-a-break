﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private float minSelectionRectSize = 10f;
    
    public static SelectionController Instance { get; private set; }
    public List<Employee> SelectedEmployees { get; } = new List<Employee>();

    public event Action SelectionChanged;

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
            
            var rect = FromDragPoints(startMousePos.Value, currentMousePos.Value);

            // Otherwise this might interfere with the click detection on individual employees
            if (rect.height < minSelectionRectSize || rect.width < minSelectionRectSize) return;
            
            GUI.Box(rect, string.Empty, selectionBoxStyle);
            
            // Do this only after we "stop" selecting? E.g. continuously or just a single time?
            FindEmployeesInRect(rect);
        }
        else
        {
            startMousePos = null;
        }
    }
    
    private Rect FromDragPoints(Vector2 p1, Vector2 p2)
    {
        Vector2 diff = p1 - p2;
        var rect = new Rect();
        if (diff.x < 0) rect.x = p1.x;
        else rect.x = p2.x;
        if (diff.y < 0) rect.y = p1.y;
        else rect.y = p2.y;
        rect.width = Mathf.Abs(diff.x);
        rect.height = Mathf.Abs(diff.y);
        return rect;
    }

    private void FindEmployeesInRect(Rect rect)
    {
        var screenRect = GUIUtility.GUIToScreenRect(rect);
        var employees = FindObjectsOfType<Employee>().Where(CanBeSelected);
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

    private bool CanBeSelected(Employee employee)
    {
        return employee.State != EmployeeState.OverStressed;
    }

    public void OnEmployeeClicked(Employee employee)
    {
        if (SelectedEmployees.Contains(employee))
        {
            DeselectEmployee(employee);
        }
        else if (CanBeSelected(employee))
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
            employee.Died += OnEmployeeDied;
            
            SelectionChanged?.Invoke();
        }
    }

    private void OnEmployeeDied(Employee employee)
    {
        DeselectEmployee(employee);
    }

    private void DeselectEmployee(Employee employee)
    {
        SelectedEmployees.Remove(employee);
        employee.SetSelected(false);
        
        SelectionChanged?.Invoke();
    }
}
