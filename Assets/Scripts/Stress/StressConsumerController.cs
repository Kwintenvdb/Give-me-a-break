using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StressConsumerController : MonoBehaviour
{
    [SerializeField] private float stressLevel = 50;
    [SerializeField] private float stressThreshold = 100;
    [SerializeField] private float baseStressPerSecond = 0;
    [SerializeField] private List<EmployeeState> baseStressActiveStates = new List<EmployeeState>();
    
    private readonly List<StressGeneratorController> _stressGenerators = new List<StressGeneratorController>();

    public Employee Employee { get; set; }
    public float PercentageStressLevel => stressLevel / stressThreshold;
    public bool IsOverstressed => stressLevel >= stressThreshold;

    private void Start()
    {
        if (Employee == null)
        {
            throw new InvalidOperationException("StressConsumerController has no Employee");
        }
    }

    private void Update()
    {
        ApplyStressPerSecond();
    }

    private void ApplyStressPerSecond()
    {
        var stressPerSecond = 0f;
        if (baseStressActiveStates.Contains(Employee.State))
        {
            stressPerSecond += baseStressPerSecond;
        }
        
        var stressGenerators = _stressGenerators.Where(generator => generator.ActiveStates.Contains(Employee.State))
            .ToList();
        stressPerSecond += stressGenerators
            .Sum(generator => generator.StressPerSecond);
        stressPerSecond *= stressGenerators
            .Select(generator => generator.StressMultiplierPerSecond)
            .Aggregate(1f, (product, stressMultiplier) => product * stressMultiplier);

        stressLevel += stressPerSecond * Time.deltaTime;
        stressLevel = Mathf.Clamp(stressLevel, 0, stressThreshold);
    }

    private void OnTriggerEnter(Collider other)
    {
        var newStressGenerators = other.GetComponents<StressGeneratorController>();
        _stressGenerators.AddRange(newStressGenerators);
        ApplyStressFixed(newStressGenerators);
    }

    private void ApplyStressFixed(StressGeneratorController[] newStressGenerators)
    {
        foreach (StressGeneratorController stressGenerator in newStressGenerators)
        {
            stressLevel += stressGenerator.StressFixed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _stressGenerators.RemoveAll(other.GetComponents<StressGeneratorController>().Contains);
    }
}