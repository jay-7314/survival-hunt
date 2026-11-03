using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private CharacterData character;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Health playerHealth;
    [SerializeField] private Animator playerAnimator;

    private void Start()
    {
        CharacterData chosen = character;

        if (GameSession.selectedCharacter != null)
        {
            chosen = GameSession.selectedCharacter;
        }
        Debug.Log($"케릭터 :{chosen.CharacterName}");

        playerMove.ApplyCharacter(chosen);
        playerAttack.ApplyCharacter(chosen);
        playerHealth.ApplyCharacter(chosen);

        if (chosen.AnimatorController != null)
        {
            playerAnimator.runtimeAnimatorController = chosen.AnimatorController;
        }

        if (GameSession.hasCarryOver)
        {
            cardManager.LoadFromSession();                      //카드 먼저 복원
            playerHealth.SetCurrent(GameSession.carryHealth);   //그다음 체력 (순서 중요)
            GameSession.hasCarryOver = false;                   //한 번 쓰면 비움
        }
        else if (chosen.StartingCard != null)
        {
            cardManager.TrySelect(chosen.StartingCard);
        }
    }
}