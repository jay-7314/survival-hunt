using UnityEngine;

public class CardApplier : MonoBehaviour
{
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private Health playerHealth;
    [SerializeField] private AutoTargeting autoTargeting;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private CardData testCard;

    public void Apply(CardData card)
    {
        switch (card.Effect)        //카드데이터에 적어놓은 효과
        {
            case CardEffect.MoveSpeed:
                playerMove.AddMoveSpeed(card.Value);
                break;
            case CardEffect.MaxHealth:
                playerHealth.IncreaseMaxHealth(Mathf.RoundToInt(card.Value));
                break;
            case CardEffect.Range:
                autoTargeting.AddRange(card.Value);
                break;
            case CardEffect.Damage:
                playerAttack.AddDamage(Mathf.RoundToInt(card.Value));
                break;
        }
    }

    [ContextMenu("Apply Test Card")]
    private void testApply()
    {
        Apply(testCard);
    }
}
