using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public struct ButtonWithHotkey
{
    public Button button;
    public KeyCode key;
    public BreakLocation relatedBreakLocation;
}

[Serializable]
public struct BreakLocationButton
{
    public Button button;
    public BreakLocation relatedBreakLocation;
}

public class ActionButtonsController : MonoBehaviour
{
    [SerializeField] private EmployeeSpawner spawner;

    [SerializeField] private Button backToWorkButton;
    [SerializeField] private Button hireButton;

    [SerializeField] private List<BreakLocationButton> breakLocationButtons;
    [SerializeField] private List<ButtonWithHotkey> buttonsWithHotkeys;

    private void Start()
    {
        SelectionController.Instance.SelectionChanged += OnSelectionChanged;
        OnSelectionChanged();
        spawner.NumEmployeesChanged += OnNumEmployeesChanged;
        OnNumEmployeesChanged();
        
        foreach (var btnWithKey in buttonsWithHotkeys)
        {
            var button = btnWithKey.button;
            var text = button.GetComponentInChildren<Text>();
            text.text = $"{text.text} ({btnWithKey.key})";
        }
    }

    private void Update()
    {
        bool anyEmployeesSelected = SelectionController.Instance.SelectedEmployees.Count > 0;
        foreach (var breakLocationButton in breakLocationButtons)
        {
            bool isInteractable = anyEmployeesSelected && breakLocationButton.relatedBreakLocation.HasFreeSlots;
            breakLocationButton.button.interactable = isInteractable;
        }
        
        foreach (var btnWithKey in buttonsWithHotkeys)
        {
            if (btnWithKey.button.IsInteractable() && Input.GetKeyUp(btnWithKey.key))
            {
                ClickButton(btnWithKey.button);
            }
        }
    }

    private void ClickButton(Button button)
    {
        ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }

    private void OnNumEmployeesChanged()
    {
        var freeWorkStations = spawner.FindFreeWorkStations().Count();
        hireButton.interactable = freeWorkStations > 0;
    }

    private void OnSelectionChanged()
    {
        bool enabled = SelectionController.Instance.SelectedEmployees.Count > 0;
        backToWorkButton.interactable = enabled;
    }
}
