using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] private StressController stressController;
    [SerializeField] private MovementController movementController;
    [SerializeField] private float stressThreshold = 100;
    [SerializeField] private EmployeeState state;
    [SerializeField] private WorkStation workStation; // Every employee must have a reference to their work station

    private void Start()
    {
        // For debugging purposes
//        var breakLocation = FindObjectOfType<BreakLocation>();
//        AssignToBreakLocation(breakLocation);
        MoveToWorkStation();
    }

    void Update()
    {
        float stressLevel = stressController.GetStressLevel();
        if (stressLevel >= stressThreshold)
        {
            state = EmployeeState.OverStressed;
            movementController.StopWalking();
        }
        // show different visual states based on stress level
    }

    public float GetPercentageStress()
    {
        return stressController.GetStressLevel() / stressThreshold;
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
