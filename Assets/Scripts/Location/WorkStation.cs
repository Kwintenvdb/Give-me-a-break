using UnityEngine;

public class WorkStation : MonoBehaviour
{
    [SerializeField] private Transform targetSlot;
    [SerializeField] private string workstationName;

    public string WorkstationName => workstationName;

    public Transform GetTargetSlot()
    {
        return targetSlot;
    }
}
