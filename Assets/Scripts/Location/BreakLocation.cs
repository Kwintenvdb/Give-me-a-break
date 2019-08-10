using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BreakLocation : MonoBehaviour
{
    [SerializeField] private List<LocationSlot> slots = new List<LocationSlot>();
    [SerializeField] private string locationName;

    public string LocationName => locationName;

    public LocationSlot AssignEmployeeToFreeSlot(Employee employee)
    {
        var slot = GetFreeSlot();
        if (slot != null)
        {
            slot.AssignEmployee(employee);
        }
        return slot;
    }

    public void RemoveEmployee(Employee employee)
    {
        var assignedSlot = slots.FirstOrDefault(slot => slot.AssignedEmployee == employee);
        if (assignedSlot != null)
        {
            assignedSlot.RemoveAssignedEmployee();
        }
    }
    
    private LocationSlot GetFreeSlot()
    {
        return slots.FirstOrDefault(slot => !slot.IsOccupied);
    }
}
