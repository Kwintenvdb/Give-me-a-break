using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Office : MonoBehaviour
{
    void Update()
    {
        var employees = FindObjectsOfType<Employee>();
        float averagePercentageStress = employees.Average(employee => employee.GetPercentageStress());
//        Debug.Log(averagePercentageStress);
    }
}
