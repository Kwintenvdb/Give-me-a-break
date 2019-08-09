using UnityEngine;

public class WorkStation : MonoBehaviour
{
    [SerializeField] private Transform targetSlot;

    public Transform GetTargetSlot()
    {
        return targetSlot;
    }
}
