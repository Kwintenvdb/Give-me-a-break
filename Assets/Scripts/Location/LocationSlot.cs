using UnityEngine;

public class LocationSlot : MonoBehaviour
{
    public Transform Target => transform;
    public bool IsOccupied => assignedEmployee != null;
    public Employee AssignedEmployee => assignedEmployee;

    private Employee assignedEmployee;

    public void AssignEmployee(Employee employee)
    {
        assignedEmployee = employee;
        employee.Died += OnEmployeeDied;
    }

    private void OnEmployeeDied(Employee employee)
    {
        RemoveAssignedEmployee();
    }

    public void RemoveAssignedEmployee()
    {
        if (assignedEmployee != null)
        {
            assignedEmployee.Died -= OnEmployeeDied;
        }
        assignedEmployee = null;
    }
}
