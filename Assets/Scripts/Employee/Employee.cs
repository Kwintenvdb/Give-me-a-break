using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] private StressConsumerController stressConsumerController;
    [SerializeField] private MovementController movementController;
    [SerializeField] private EmployeeState state;
    [SerializeField] private WorkStation workStation; // Every employee must have a reference to their work station

    public EmployeeState State => state;

    private void Awake()
    {
        stressConsumerController.Employee = this;
    }

    private void Start()
    {
        if (workStation == null)
        {
            throw new InvalidOperationException("Employee does not have a Workstation");
        }
        
        // For debugging purposes
//        var breakLocation = FindObjectOfType<BreakLocation>();
//        AssignToBreakLocation(breakLocation);
        MoveToWorkStation();
    }

    void Update()
    {
        if (stressConsumerController.IsOverstressed)
        {
            state = EmployeeState.OverStressed;
            movementController.StopWalking();
        }
        // show different visual states based on stress level
    }

    // Should be called externally - after giving a command to a group of employees
    public void AssignToBreakLocation(BreakLocation breakLocation)
    {
        SetState(EmployeeState.Walking);
        // TODO only move if the employee is not there already
        var movementTarget = breakLocation.GetTargetSlot();
        movementController.SetMovementTarget(movementTarget, () =>
        {
            Debug.Log("Target reached");
            SetState(EmployeeState.Break);
        });
    }

    public void MoveToWorkStation()
    {
        SetState(EmployeeState.Walking);
        var movementTarget = workStation.GetTargetSlot();
        movementController.SetMovementTarget(movementTarget, () =>
        {
            Debug.Log("Workstation reached");
            SetState(EmployeeState.Working);
        });
    }

    private void SetState(EmployeeState state)
    {
        this.state = state;
    }
}
