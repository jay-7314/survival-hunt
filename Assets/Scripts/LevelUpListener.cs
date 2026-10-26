using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpListener : MonoBehaviour
{
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private CardPicker cardPicker;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private TextMeshProUGUI[] cardTexts;
    [SerializeField] private CardApplier cardApplier;

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
        if (currentCards == null && index >= currentCards.Count) return;

        cardApplier.Apply(currentCards[index]);
        cardPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
