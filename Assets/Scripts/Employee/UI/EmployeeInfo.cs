using UnityEngine;
using UnityEngine.UI;

public class EmployeeInfo : MonoBehaviour
{
    [SerializeField] private Employee employee;
    [SerializeField] private Text nameText;
    [SerializeField] private Text stressLevelText;
    [SerializeField] private Text stateText;
    [SerializeField] private RectTransform panel;

    private bool isExpanded = false;

    private void Start()
    {
        nameText.text = employee.EmployeeName;
    }

    private void Update()
    {
        float stressAsPercentage = employee.PercentageStressLevel * 100;
        stressLevelText.text = isExpanded
            ? $"{stressAsPercentage:N}% stressed"
            : $"{stressAsPercentage:N}%";

        if (isExpanded)
        {
            stateText.text = GetStateText(employee);
        }
    }

    private string GetStateText(Employee employee)
    {
        switch (employee.State)
        {
            case EmployeeState.Walking:
                string target = employee.AssignedBreakLocation != null
                    ? employee.AssignedBreakLocation.LocationName
                    : employee.AssignedWorkStation.WorkstationName;
                return "Walking to " + target;
            case EmployeeState.Working:
                return "Working";
            case EmployeeState.Break:
                return "Taking a break";
            case EmployeeState.OverStressed:
                return "About to explode";
        }

        return string.Empty;
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void SetExpanded(bool expanded)
    {
        isExpanded = expanded;
        stateText.gameObject.SetActive(expanded);
        nameText.gameObject.SetActive(expanded);
        panel.sizeDelta = new Vector2(expanded ? 250 : 120, expanded ? 100 : 42);
    }
}