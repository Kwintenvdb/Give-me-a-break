using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    
    public void SetMoney(double money)
    {
        moneyText.text = money.ToString("C");
    }
}
