using UnityEngine;

public class EmployeeStressVisuals : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite happy;
    [SerializeField] private Sprite satisfied;
    [SerializeField] private Sprite average;
    [SerializeField] private Sprite sad;
    [SerializeField] private Sprite exhausted;
    [SerializeField] private Sprite scared;
    [SerializeField] private Sprite superScared;
    
    public void SetStressLevel(float stressLevel)
    {
        var sprite = GetSprite(stressLevel);
        renderer.sprite = sprite;
    }

    private Sprite GetSprite(float stressLevel)
    {
        animator.SetBool("Pulsate", false);
        if (stressLevel <= 0.1f)
        {
            return happy;
        }
        if (stressLevel <= 0.2f)
        {
            return satisfied;
        }
        if (stressLevel <= 0.3f)
        {
            return average;
        }
        if (stressLevel <= 0.45f)
        {
            return sad;
        }
        if (stressLevel <= 0.65f)
        {
            return exhausted;
        }
        if (stressLevel <= 0.8f)
        {
            animator.SetBool("Pulsate", true);
            return scared;
        }

        animator.SetBool("Pulsate", true);
        return superScared;
    }
}
