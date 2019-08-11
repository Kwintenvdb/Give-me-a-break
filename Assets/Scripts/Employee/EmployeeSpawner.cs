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

    private List<WorkStation> _workStations;
//    private List<Employee> employees = new List<Employee>();
    
    private void Awake()
    {
        _workStations = FindObjectsOfType<WorkStation>().ToList();
        StartCoroutine(SpawnEmployees());
    }

    // Start of day
    private IEnumerator SpawnEmployees()
    {
        foreach (var workStation in _workStations
            .OrderBy(a => Random.Range(0,1))
            .Take(initialWorkerCount))
        {
            SpawnEmployee(workStation);
            yield return new WaitForSeconds(1f);
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
    }

    public void HireNewEmployee()
    {
        var occupiedWorkStations = FindObjectsOfType<Employee>()
            .Select(employee => employee.AssignedWorkStation);
        var freeWorkStation = _workStations
            .Where(station => !occupiedWorkStations.Contains(station))
            .OrderBy(a => Random.Range(0, 1))
            .First();
        
        // add check if no workstation free
        SpawnEmployee(freeWorkStation);
    }
}
