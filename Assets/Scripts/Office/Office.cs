using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Office : MonoBehaviour
{
    [SerializeField] private BreakLocation lunchRoom;
    [SerializeField] private BreakLocation toilets;
    [SerializeField] private float moneyBalance = 0;
    [SerializeField] private BackgroundMusicController backgroundMusicController;

    // Office State
    [SerializeField] private OfficeState state;
    [SerializeField] private float officeStateTenseLevel = 0.33f;
    [SerializeField] private float officeStateRiotLevel = 0.66f;

    // UI
    [SerializeField] private StressBar stressBar;
    [SerializeField] private MoneyDisplay moneyDisplay;

    public OfficeState State => state;

    void Update()
    {
        var employees = FindObjectsOfType<Employee>();
        CalculateAveragePercentageStress(employees);
        UpdateMoneyBalance(employees);
    }

    private void CalculateAveragePercentageStress(IEnumerable<Employee> employees)
    {
        float averagePercentageStress = employees
            .Select(employee => employee.StressConsumerController)
            .Average(x => x.PercentageStressLevel);
        stressBar.SetStressLevel(averagePercentageStress);
        UpdateOfficeState(averagePercentageStress);
    }

    private void UpdateOfficeState(float averagePercentageStress)
    {
        if (averagePercentageStress > officeStateRiotLevel)
        {
            state = OfficeState.Riot;
        }
        else if (averagePercentageStress > officeStateTenseLevel)
        {
            state = OfficeState.Tense;
        }
        else
        {
            state = OfficeState.Chill;
        }
    }

    private void UpdateMoneyBalance(IEnumerable<Employee> employees)
    {
        moneyBalance += employees
            .Select(employee => employee.MoneyConsumerController)
            .Sum(moneyMaker => moneyMaker.CalculateMoneyGenerated());
        moneyDisplay.SetMoney(moneyBalance);
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