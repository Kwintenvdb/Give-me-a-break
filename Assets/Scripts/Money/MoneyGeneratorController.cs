using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyGeneratorController : MonoBehaviour
{
    [SerializeField] private float moneyPerSecond;
    [SerializeField] private float moneyMultiplierPerSecond;
    [SerializeField] private List<EmployeeState> activeStates = new List<EmployeeState>{EmployeeState.Working};

    public float MoneyPerSecond => moneyPerSecond;

    public float MoneyMultiplierPerSecond => moneyMultiplierPerSecond;

    public List<EmployeeState> ActiveStates => activeStates;
}
