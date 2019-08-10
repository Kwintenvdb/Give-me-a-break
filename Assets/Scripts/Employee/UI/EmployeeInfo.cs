using UnityEngine;
using UnityEngine.UI;

public class EmployeeInfo : MonoBehaviour
{
    [SerializeField] private Employee employee;
    [SerializeField] private Text nameText;
    [SerializeField] private Text stressLevelText;
    [SerializeField] private Text stateText;

    private void Awake()
    {
        nameText.text = employee.EmployeeName;
    }

    private void Update()
    {
        float stressAsPercentage = employee.PercentageStressLevel * 100;
        stressLevelText.text = string.Format("{0:N}% stressed", stressAsPercentage);
        stateText.text = GetStateText(employee);
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
}
