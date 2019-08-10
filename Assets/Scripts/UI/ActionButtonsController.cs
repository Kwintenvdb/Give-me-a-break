using UnityEngine;

public class ActionButtonsController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        SelectionController.Instance.SelectionChanged += OnSelectionChanged;
        OnSelectionChanged();
    }

    private void OnSelectionChanged()
    {
        bool enabled = SelectionController.Instance.SelectedEmployees.Count > 0;
        canvasGroup.interactable = enabled;
    }
}
