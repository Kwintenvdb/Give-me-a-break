using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    
    public void SetMoney(float money)
    {
        moneyText.text = money.ToString("C");
    }
}
