using System.Collections.Generic;
using UnityEngine;

public class StressGeneratorController : MonoBehaviour
{
    [SerializeField] private float stressPerSecond;
    [SerializeField] private float stressMultiplierPerSecond;
    [SerializeField] private float stressFixed;
    [SerializeField] private List<EmployeeState> activeStates;

    public float StressPerSecond => stressPerSecond;

    public float StressMultiplierPerSecond => stressMultiplierPerSecond;

    public float StressFixed => stressFixed;

    public List<EmployeeState> ActiveStates => activeStates;
}
