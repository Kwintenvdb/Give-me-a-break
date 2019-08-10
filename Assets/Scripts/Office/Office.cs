using System;
using System.Linq;
using UnityEngine;

public class Office : MonoBehaviour
{
    [SerializeField] private BreakLocation lunchRoom;
    [SerializeField] private BreakLocation toilets;
    
    void Update()
    {
        var employees = FindObjectsOfType<StressConsumerController>();
        float averagePercentageStress = employees.Average(x => x.PercentageStressLevel);
//        Debug.Log(averagePercentageStress);
    }

    public void SendSelectedToWork()
    {
        ForEachSelected(employee => employee.MoveToWorkStation());
    }

    public void SendSelectedToLunch()
    {
        ForEachSelected(employee => employee.AssignToBreakLocation(lunchRoom));
    }

    public void SendSelectedToToilet()
    {
        ForEachSelected(employee => employee.AssignToBreakLocation(toilets));
    }

    private void ForEachSelected(Action<Employee> action)
    {
        var selected = SelectionController.Instance.SelectedEmployees;
        foreach (var employee in selected)
        {
            action(employee);
        }
    }
}
