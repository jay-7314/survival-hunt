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

        if(GameSession.selectedCharacter != null)
        {
            chosen = GameSession.selectedCharacter;
        }
        Debug.Log($"케릭터 :{chosen.CharacterName}");

        playerMove.ApplyCharacter(chosen);
        playerAttack.ApplyCharacter(chosen);
        playerHealth .ApplyCharacter(chosen);

        if(chosen.AnimatorController != null)
        {
            playerAnimator.runtimeAnimatorController = chosen.AnimatorController;
        }

        //고른 캐릭터를 검사
        if(character.StartingCard != null)
        {
            cardManager.TrySelect(chosen.StartingCard);
        }
    }
}
