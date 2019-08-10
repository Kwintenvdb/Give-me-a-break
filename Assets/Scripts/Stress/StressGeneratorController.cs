using System.Collections.Generic;
using UnityEngine;

public class StressGeneratorController : MonoBehaviour
{
    [SerializeField] private float stressPerSecond;
    [SerializeField] private float stressMultiplierPerSecond = 1;
    [SerializeField] private float stressFixed;
    [SerializeField] private List<EmployeeState> activeStates;

    [SerializeField] private Employee employee;

    public float StressPerSecond()
    {
        if (employee == null || activeStates.Contains(employee.State))
            return stressPerSecond;
        else
            return 0f;
    }

    public float StressMultiplierPerSecond()
    {
        if (employee == null || activeStates.Contains(employee.State))
            return stressMultiplierPerSecond;
        else
            return 1f;

    }

    public float StressFixed => stressFixed;

    public List<EmployeeState> ActiveStates => activeStates;
}
