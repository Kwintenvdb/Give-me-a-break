using System.Collections.Generic;
using UnityEngine;

public class StressGeneratorController : MonoBehaviour
{
    [SerializeField] private int stressPerSecond;
    [SerializeField] private int stressMultiplierPerSecond;
    [SerializeField] private int stressFixed;
    [SerializeField] private List<EmployeeState> activeStates;

    public int StressPerSecond => stressPerSecond;

    public int StressMultiplierPerSecond => stressMultiplierPerSecond;

    public int StressFixed => stressFixed;

    public List<EmployeeState> ActiveStates => activeStates;
}
