using UnityEngine;
using UnityEngine.UI;

public class StressBar : MonoBehaviour
{
    [SerializeField] private Image bar;
    
    public void SetStressLevel(float percentage)
    {
        bar.fillAmount = percentage;
    }
}
