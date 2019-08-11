using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EmployeeSpawner : MonoBehaviour
{
    [SerializeField] private Employee employeePrefab;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private float minStressLevel = 10;
    [SerializeField] private float maxStressLevel = 30;
    [SerializeField] private int initialWorkerCount = 25;

    [SerializeField] private float employeeSpawnTimeMin = 0.5f;
    [SerializeField] private float employeeSpawnTimeMax = 1f;

    private List<WorkStation> _workStations;
    public List<Employee> Employees { get; } = new List<Employee>();
    public int EmployeesDied { get; private set; }
    
    private void Awake()
    {
        _workStations = FindObjectsOfType<WorkStation>().ToList();
        StartCoroutine(SpawnEmployees());
    }

    // Start of day
    private IEnumerator SpawnEmployees()
    {
        foreach (var workStation in _workStations
            .OrderBy(a => Random.Range(0, 1))
            .Take(initialWorkerCount))
        {
            SpawnEmployee(workStation);
            float timeTillNextSpawn = Random.Range(employeeSpawnTimeMin, employeeSpawnTimeMax);
            yield return new WaitForSeconds(timeTillNextSpawn);
        }
    }

    // Spawn at door
    private void SpawnEmployee(WorkStation assignedWorkstation)
    {
        var employee = Instantiate(employeePrefab, spawnLocation.position, Quaternion.identity);
        employee.AssignWorkStation(assignedWorkstation);
        employee.MoveToWorkStation();

        float baseStress = Random.Range(minStressLevel, maxStressLevel);
        employee.StressConsumerController.SetBaseStress(baseStress);
        
        Employees.Add(employee);
        RegisterEmployeeDeathEvent(employee);
    }

    private void RegisterEmployeeDeathEvent(Employee employee)
    {
        employee.Died += e =>
        {
            e.RemoveAssignedWorkStation();
            Employees.Remove(e);
            EmployeesDied++;
        };
    }

    public void HireNewEmployee()
    {
        var occupiedWorkStations = Employees
            .Select(employee => employee.AssignedWorkStation);
        var freeWorkStation = _workStations
            .Where(station => !occupiedWorkStations.Contains(station))
            .OrderBy(a => Random.Range(0, 1))
            .First();
        
        // TODO add check if no workstation free
        SpawnEmployee(freeWorkStation);
    }
}
