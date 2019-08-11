using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonsController : MonoBehaviour
{
    [SerializeField] private EmployeeSpawner spawner;
    
    [SerializeField] private Button[] buttonsToToggle;
    [SerializeField] private Button hireButton;

    private void Start()
    {
        SelectionController.Instance.SelectionChanged += OnSelectionChanged;
        OnSelectionChanged();
        spawner.NumEmployeesChanged += OnNumEmployeesChanged;
        OnNumEmployeesChanged();
    }

    private void OnNumEmployeesChanged()
    {
        var freeWorkStations = spawner.FindFreeWorkStations().Count();
        hireButton.interactable = freeWorkStations > 0;
    }

    private void OnSelectionChanged()
    {
        bool enabled = SelectionController.Instance.SelectedEmployees.Count > 0;
        foreach (var button in buttonsToToggle)
        {
            button.interactable = enabled;
        }
    }
}
