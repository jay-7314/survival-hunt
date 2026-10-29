using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private CharacterData character;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Health playerHealth;

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

        //기본 능력치를 일단 적용하기 위해서 함수의 위치도 중요함.
        if(character.StartingCard != null)
        {
            cardManager.TrySelect(chosen.StartingCard);
        }
    }
}
