using UnityEngine;
using UnityEngine.UI;

public class ActionButtonsController : MonoBehaviour
{
    [SerializeField] private Button[] buttonsToToggle;

    private void Start()
    {
        SelectionController.Instance.SelectionChanged += OnSelectionChanged;
        OnSelectionChanged();
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
