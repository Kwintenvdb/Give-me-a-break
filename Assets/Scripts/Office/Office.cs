using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Office : MonoBehaviour
{
    [SerializeField] private BreakLocation lunchRoom;
    [SerializeField] private BreakLocation toilets;
    [SerializeField] private float moneyBalance = 0;

    [SerializeField] private StressBar stressBar;
    
    void Update()
    {
        var employees = FindObjectsOfType<Employee>();
        CalculateAveragePercentageStress(employees);
        moneyBalance += employees
            .Select(employee => employee.MoneyConsumerController)
            .Sum(moneyMaker => moneyMaker.CalculateMoneyGenerated());
    }

    private void CalculateAveragePercentageStress(IEnumerable<Employee> employees)
    {
        float averagePercentageStress = employees
            .Select(employee => employee.StressConsumerController)
            .Average(x => x.PercentageStressLevel);
        stressBar.SetStressLevel(averagePercentageStress);
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
