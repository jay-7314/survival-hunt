using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpListener : MonoBehaviour
{
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private CardPicker cardPicker;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private TextMeshProUGUI[] cardTexts;
    [SerializeField] private Image[] cardImages;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GameObject replacePrompt;
    [SerializeField] private Health playerHealth;
    [SerializeField] private int skipHealAmount = 2;

    [SerializeField] private Sprite fireCardSprite;
    [SerializeField] private Sprite waterCardSprite;
    [SerializeField] private Sprite earthCardSprite;
    [SerializeField] private Sprite windCardSprite;

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
        for (int i = 0; i < currentCards.Count && i < currentCards.Count; i++)
        {
            cardTexts[i].text = $"{currentCards[i].CardName}\n{currentCards[i].Description}";

            Sprite s = CardSpriteOf(currentCards[i].CardElement);
            if (s != null) cardImages[i].sprite = s;
            cardImages[i].color = Color.white;
        }
        SoundManager.Instance.Play(Sfx.LevelUp);
        cardPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SelectCard(int index)
    {
        //    if (pendingCard != null) return;
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
    private Sprite CardSpriteOf(Element e)
    {
        switch (e)
        {
            case Element.Fire: return fireCardSprite;
            case Element.Water: return waterCardSprite;
            case Element.Earth: return earthCardSprite;
            case Element.Wind: return windCardSprite;
            default: return null;
        }
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

    public void Skip()
    {
        playerHealth.Heal(skipHealAmount);
        Debug.Log("카드 건너뛰기");
         CloseLevelUp() ;
    }
}
