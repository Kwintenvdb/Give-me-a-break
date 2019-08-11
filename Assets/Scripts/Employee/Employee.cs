using UnityEngine.EventSystems;
using System;
using System.Collections;
using UnityEngine;

public class Employee : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private StressConsumerController stressConsumerController;
    [SerializeField] private MoneyConsumerController moneyConsumerController;
    [SerializeField] private MovementController movementController;
    [SerializeField] private EmployeeState state;
    [SerializeField] private WorkStation workStation; // Every employee must have a reference to their work station
    [SerializeField] private string employeeName;
    [SerializeField] private EmployeeInfo employeeInfo;
    [SerializeField] private GameObject selectionObject;
    [SerializeField] private EmployeeStressVisuals stressVisuals;
    [SerializeField] private GameObject renderer;
    [SerializeField] private ParticleSystem explosionPrefab;

    public StressConsumerController StressConsumerController => stressConsumerController;
    public MoneyConsumerController MoneyConsumerController => moneyConsumerController;
    public EmployeeState State => state;
    public string EmployeeName => employeeName;
    public float PercentageStressLevel => stressConsumerController.PercentageStressLevel;

    public WorkStation AssignedWorkStation => workStation;
    public BreakLocation AssignedBreakLocation { get; private set; }

    public event Action<Employee> Died;

    private void Awake()
    {
        stressConsumerController.Employee = this;
        moneyConsumerController.Employee = this;
        
        SetSelected(false);
        employeeInfo.SetExpanded(false);
    }

    private void Start()
    {
        if (workStation == null)
        {
            throw new InvalidOperationException("Employee does not have a Workstation");
        }
    }

    void Update()
    {
        stressVisuals.SetStressLevel(PercentageStressLevel, state, AssignedBreakLocation);
        
        if (stressConsumerController.IsOverstressed && state != EmployeeState.OverStressed)
        {
            Died?.Invoke(this);
            
            SetState(EmployeeState.OverStressed);
            movementController.StopWalking();
            Destroy(gameObject);
        }
    }

    private bool isQuitting = false;
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (isQuitting) return;
        var pos = transform.position + new Vector3(0, 2.5f, 0);
        Instantiate(explosionPrefab, pos, Quaternion.identity);
    }

    public void SendOnVacation(BreakLocation vacation)
    {
        AssignToBreakLocation(vacation, () => { StartCoroutine(DisappearForVacation()); });
    }

    private IEnumerator DisappearForVacation()
    {
        SetVisible(false);
        SelectionController.Instance.DeselectEmployee(this);
        yield return new WaitForSeconds(15);
        SetVisible(true);
        MoveToWorkStation();
    }

    private void SetVisible(bool visible)
    {
        renderer.SetActive(visible);
        employeeInfo.SetVisible(visible);
    }
    
    // Should be called externally - after giving a command to a group of employees
    public void AssignToBreakLocation(BreakLocation breakLocation, Action locationReached = null)
    {
        RemoveFromAssignedBreakLocation();
        
        var slot = breakLocation.AssignEmployeeToFreeSlot(this);
        if (slot != null)
        {
            AssignedBreakLocation = breakLocation;
            
            SetState(EmployeeState.Walking);
            // TODO only move if the employee is not there already
            var movementTarget = slot.Target;
            movementController.SetMovementTarget(movementTarget, () =>
            {
                SetState(EmployeeState.Break);
                locationReached?.Invoke();
            });
        }
    }

    private void RemoveFromAssignedBreakLocation()
    {
        if (AssignedBreakLocation != null)
        {
            AssignedBreakLocation.RemoveEmployee(this);
            AssignedBreakLocation = null;
        }
    }

    public void AssignWorkStation(WorkStation workStation)
    {
        this.workStation = workStation;
    }

    public void RemoveAssignedWorkStation()
    {
        workStation = null;
    }
    
    public void MoveToWorkStation()
    {
        RemoveFromAssignedBreakLocation();
        
        SetState(EmployeeState.Walking);
        var movementTarget = workStation.GetTargetSlot();
        movementController.SetMovementTarget(movementTarget, () =>
        {
            SetState(EmployeeState.Working);
        });
    }

    public void SetName(string name)
    {
        employeeName = name;
    }

    private void SetState(EmployeeState state)
    {
        this.state = state;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectionController.Instance.OnEmployeeClicked(this);
    }

    public void SetSelected(bool selected)
    {
        selectionObject.SetActive(selected);
    }

    // Hover handlers
    public void OnPointerEnter(PointerEventData eventData)
    {
        employeeInfo.SetExpanded(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        employeeInfo.SetExpanded(false);
    }
}
