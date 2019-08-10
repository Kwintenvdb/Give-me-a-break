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
    }

    public void RemoveAssignedEmployee()
    {
        assignedEmployee = null;
    }
}
