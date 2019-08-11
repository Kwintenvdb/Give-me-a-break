using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private Text moneyEarnedText;
    [SerializeField] private Text employeesKilledText;
    
    private void Awake()
    {
        moneyEarnedText.text = $"You made {GoalManager.MoneyEarned:N} Catcoins!";
        employeesKilledText.text = $"...and killed {GoalManager.EmployeesKilled} cats in the process.";
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
