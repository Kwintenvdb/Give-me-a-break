using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] private StressConsumerController stressConsumerController;
    [SerializeField] private EmployeeState state;

    public EmployeeState State => state;

    private void Awake()
    {
        if (stressConsumerController != null)
        {
            stressConsumerController.Employee = this;
        }
    }

    void Update()
    {
        if (stressConsumerController.IsOverstressed())
        {
            state = EmployeeState.OverStressed;
        }
        // show different visual states based on stress level
    }

}
