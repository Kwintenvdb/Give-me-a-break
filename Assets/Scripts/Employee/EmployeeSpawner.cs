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

    [SerializeField] private float employeeSpawnTimeMinMorning = 0.2f;
    [SerializeField] private float employeeSpawnTimeMaxMorning = 0.8f;
    
    [SerializeField] private float employeeSpawnTimeMin = 1;
    [SerializeField] private float employeeSpawnTimeMax = 4;

    private List<String> employeeNames = new List<string>
    {
        "Fabian", "Abdurrahman", "Mohamed", "Jan", "Stefan", "Harald", "Manuel", "Mina", "Balazs", "Ion", "Michael",
        "Maximilian", "Robert", "Tamás", "Rosana", "Goga", "Dominik", "Matthias", "Benjamin", "Flaviu", "Tobias",
        "Aurelian", "Dennis", "Andreea", "Vasile", "Enikö", "Alexandru", "Arif", "Luis", "Cristian", "David", "Adrian",
        "Bogdan", "Cristian", "Paul", "Catalin", "Liviu", "Ovidiu", "Iulia", "Hunor", "Simon", "Artur", "Emil", "Peter",
        "Dominik", "Marc", "Manuel", "Matthias", "Johannes", "Abdelrahman", "Ibrahim", "Jakob", "Philippos",
        "Alexander", "Markus", "Andreas", "David", "Andrei", "Manuel", "Ana", "Clemens", "Stefan", "Patric", "David",
        "Konstantin", "Paul", "Anne", "Daniela", "Philipp", "Jonas", "Matthias", "Kilian", "Matthias", "Jakob",
        "Hermine", "Matthias", "Brigitte", "Niklas", "Gabriel", "Viorel", "David", "Jovan", "Michael", "Michal",
        "Thomas", "Jeremy", "Jakob", "Maria", "Marcel", "Stefan", "Florian", "Georg", "Simon", "Denisa", "Peer",
        "Manuel", "Mario", "Simon", "Florian", "Gustav", "Engy", "Markus", "Vibeesh", "Alexander", "Stefan", "Bernhard",
        "Alexandra", "Zied", "Andreea", "Markus", "Ondrej", "Diana", "Iosif-Alexandru", "Patrick", "Arnold", "Benjamin",
        "Karin", "Wolfgang", "Lukas", "Catalin", "Michael", "Roxana", "Anamaria-Roberta", "Omar", "Manuel", "Clemens",
        "Mohammad", "Bernhard", "Thomas", "Philipp", "Dominik", "Maurice", "Nikita", "Baher", "Bogdan", "Florian",
        "Christoph", "Remo", "Zoltan", "David", "Stefan", "Markus", "Fabian", "Felix", "Florian", "Mathias", "Claudia",
        "Vincenzo", "Raphael", "Haytham", "Bedanand", "Daniel", "Cristian", "Ioan", "Stefan", "Michael", "Peter",
        "Robert", "Robin", "Florian", "Raul", "Stefan", "Katrin", "Yannick", "Noah", "Siegfried", "Csongor", "Dominik",
        "Sibel", "Bristina", "Jasmin", "Thomas", "Alexandru", "Christopher", "Kwinten", "Alin", "Bozo", "Moritz",
        "Martin", "Christoph", "Christian", "Nader", "David", "Jonas", "Stefan", "Jakob", "Jakob", "Florian", "Kangrui"
    };
        
    private List<WorkStation> _workStations;
    public List<Employee> Employees { get; } = new List<Employee>();
    public int EmployeesDied { get; private set; }

    public event Action NumEmployeesChanged;
    
    private void Awake()
    {
        _workStations = FindObjectsOfType<WorkStation>().ToList();
        StartCoroutine(SpawnInitialEmployees());
        StartCoroutine(SpawnEmployees());
    }

    // Start of day
    private IEnumerator SpawnInitialEmployees()
    {
        foreach (var workStation in _workStations
            .OrderBy(a => Random.Range(0, 1))
            .Take(initialWorkerCount))
        {
            if (IsWorkStationFree(workStation))
            {
                SpawnEmployee(workStation);
                float timeTillNextSpawn = Random.Range(employeeSpawnTimeMinMorning, employeeSpawnTimeMaxMorning);
                yield return new WaitForSeconds(timeTillNextSpawn);
            }
        }
    }

    private IEnumerator SpawnEmployees()
    {
        while (true)
        {
            HireNewEmployee();
            float timeTillNextSpawn = Random.Range(employeeSpawnTimeMin, employeeSpawnTimeMax);
            yield return new WaitForSeconds(timeTillNextSpawn);    
        }
    }

    private void SpawnEmployee(WorkStation assignedWorkstation)
    {
        string employeeName = employeeNames[Random.Range(0, employeeNames.Count)];
        var employee = Instantiate(employeePrefab, spawnLocation.position, Quaternion.identity);
        employee.SetName(employeeName);
        employee.AssignWorkStation(assignedWorkstation);
        employee.MoveToWorkStation();

        float baseStress = Random.Range(minStressLevel, maxStressLevel);
        employee.StressConsumerController.SetBaseStress(baseStress);
        
        Employees.Add(employee);
        NumEmployeesChanged?.Invoke();
        RegisterEmployeeDeathEvent(employee);
    }

    private void RegisterEmployeeDeathEvent(Employee employee)
    {
        employee.Died += e =>
        {
            e.RemoveAssignedWorkStation();
            Employees.Remove(e);
            EmployeesDied++;
            NumEmployeesChanged?.Invoke();
        };
    }

    public void HireNewEmployee()
    {
        var freeWorkStation = FindFreeWorkStations()
            .OrderBy(a => Random.Range(0, 1))
            .FirstOrDefault();
        if (freeWorkStation != null)
        {
            SpawnEmployee(freeWorkStation);
        }
    }

    public IEnumerable<WorkStation> FindFreeWorkStations()
    {
        return _workStations.Where(IsWorkStationFree);
    }

    private bool IsWorkStationFree(WorkStation workStation)
    {
        var occupiedWorkStations = Employees.Select(employee => employee.AssignedWorkStation);
        return !occupiedWorkStations.Contains(workStation);
    }
}
