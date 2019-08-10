using UnityEngine.EventSystems;
using System;
using UnityEngine;

public class Employee : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private StressConsumerController stressConsumerController;
    [SerializeField] private MoneyConsumerController moneyConsumerController;
    [SerializeField] private MovementController movementController;
    [SerializeField] private EmployeeState state;
    [SerializeField] private WorkStation workStation; // Every employee must have a reference to their work station
    [SerializeField] private Renderer renderer;

    public StressConsumerController StressConsumerController => stressConsumerController;
    public MoneyConsumerController MoneyConsumerController => moneyConsumerController;
    public EmployeeState State => state;

    private void Awake()
    {
        stressConsumerController.Employee = this;
        moneyConsumerController.Employee = this;
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
//        MoveToWorkStation();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectionController.Instance.OnEmployeeClicked(this);
    }

    public void SetSelected(bool selected)
    {
        renderer.material.color = selected ? Color.blue : Color.white;
    }
}
