using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotBarUI : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private TextMeshProUGUI[] slotTexts;
    [SerializeField] private Image[] slotImages;
    [SerializeField] private Color emptyColor = new Color(1f, 1f, 1f, 0.3f);

    private void OnEnable()
    {
        cardManager.SlotsChanged += Refresh;
    }

    private void OnDisable()
    {
        cardManager.SlotsChanged -= Refresh;
    }


    private void Refresh()
    {
        for (int i = 0; i < slotTexts.Length && i < CardManager.SlotCount; i++)
        {
            CardSlot slot = cardManager.getSlot(i);

            if (slot.IsEmpty)
            {
                slotTexts[i].text = "";
                slotImages[i].color = emptyColor;
            }
            else
            {
                slotTexts[i].text = $"{slot.card.CardName}\nLv{slot.level}";
                slotImages[i].color = ElementColors.Get(slot.card.CardElement);
            }
        }
    }
}
