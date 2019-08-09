using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakLocation : MonoBehaviour
{
    [SerializeField] private Transform targetSlot;

    public Transform GetTargetSlot()
    {
        return targetSlot;
    }
}
