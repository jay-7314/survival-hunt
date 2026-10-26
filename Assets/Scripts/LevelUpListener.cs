using System.Collections.Generic;
using UnityEngine;

public class LevelUpListener : MonoBehaviour
{
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private CardPicker cardPicker;

    private void OnEnable()
    {
        playerLevel.LeveledUp += HandleLeveledUp;
    }

    private void OnDisable()
    {
        playerLevel.LeveledUp -= HandleLeveledUp;
    }

    private void HandleLeveledUp(int newLevel)
    {
        Debug.Log($"카드 3장을 고르세요! (레벨 {newLevel})");
        List<CardData> cards = cardPicker.PickRandom(3);
        for(int i = 0; i<cards.Count; i++)
        {
            Debug.Log($"{i + 1}번 : {cards[i].CardName} ( {cards[i].CardElement} )");
        }
    }
}
