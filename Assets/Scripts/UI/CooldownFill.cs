using UnityEngine;
using UnityEngine.UI;

public class CooldownFill : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private MeleeAttack meleeAttack;

    private void Update()
    {
        float ratio = 0f;
        if (playerAttack != null) ratio = playerAttack.CooldownRatio;
        else if (meleeAttack != null) ratio = meleeAttack.CooldownRatio;

        fillImage.fillAmount = ratio;
    }
}
