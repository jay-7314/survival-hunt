using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SlotBarUI : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private TextMeshProUGUI[] slotTexts;

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
        for(int i = 0; i<slotTexts.Length; i++)
        {
            CardSlot slot = cardManager.getSlot(i);

            if (slot.IsEmpty)
            {
                slotTexts[i].text = "";
            }
            else
            {
                slotTexts[i].text = $"{slot.card.CardName}\nLv{slot.level}";
            }
        }
    }
}
