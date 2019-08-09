using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressController : MonoBehaviour
{
    [SerializeField] private float stressLevel = 50;
    [SerializeField] private float stressGrowthPerSecond = 2;

    // Update is called once per frame
    void Update()
    {
        stressLevel += stressGrowthPerSecond * Time.deltaTime;
    }

    public float GetStressLevel()
    {
        return stressLevel;
    }
}
