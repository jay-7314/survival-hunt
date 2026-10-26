using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpListener : MonoBehaviour
{
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private CardPicker cardPicker;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private TextMeshProUGUI[] cardTexts;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GameObject replacePrompt;

    private CardData pendingCard;
    private List<CardData> currentCards;

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
        pendingCard = null;
        replacePrompt.SetActive(false);
        currentCards = cardPicker.PickRandom(3);
        for(int i = 0; i< currentCards.Count && i< currentCards.Count; i++)        //글자칸이나 카드수보다 많이 접근해서 에러가 나지 않게 하기
        {
            cardTexts[i].text = $"{currentCards[i].CardName}\n{currentCards[i].Description}";
        }

        cardPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SelectCard(int index)
    {
        if (pendingCard != null) return;
        if (currentCards == null || index >= currentCards.Count) return;

        CardData card = currentCards[index];
        CardManager.SelectResult result = cardManager.TrySelect(card);

        if(result == CardManager.SelectResult.NeedReplace)
        {
            pendingCard = card;
            replacePrompt.SetActive(true);
            Debug.Log("교체할 슬롯을 선택하세요");
            return;
        }
        CloseLevelUp();
    }

    public void SelectSlot(int slotIndex)
    {
        if (pendingCard == null) return;
        cardManager.Replace(slotIndex, pendingCard);
        pendingCard = null;
        CloseLevelUp();
    }

    private void CloseLevelUp()
    {
        replacePrompt.SetActive(false);
        cardPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CancelReplace()
    {
        pendingCard = null;
        replacePrompt.SetActive(false);
    }
}
