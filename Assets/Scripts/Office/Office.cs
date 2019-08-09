using System.Linq;
using UnityEngine;

public class Office : MonoBehaviour
{
    void Update()
    {
        var employees = FindObjectsOfType<StressConsumerController>();
        float averagePercentageStress = employees.Average(x => x.PercentageStressLevel);
//        Debug.Log(averagePercentageStress);
    }
}
