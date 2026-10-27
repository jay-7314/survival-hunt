using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public const int SlotCount = 4;
    [SerializeField] private CardSlot[] slots;
    [SerializeField] private CardData testCard;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private AutoTargeting autoTargeting;
    [SerializeField] private Health playerHealth;

    public event Action SlotsChanged;
    private int[] elementCounts = new int[5];

    private void CountElements()
    {
        //먼저 전부 0으로
        for(int i =0; i<elementCounts.Length; i++)
        {
            elementCounts[i] = 0;
        }

        for(int i =0; i<slots.Length; i++)
        {
            if (slots[i].IsEmpty) continue;
            int e = (int)slots[i].card.CardElement;
            elementCounts[e] += 1;
        }
        Debug.Log($"불 : {elementCounts[1]}, 물 : {elementCounts[2]}, 땅 :{elementCounts[3]}, 바람 : {elementCounts[4]}");
    }

    public CardSlot getSlot(int index)
    {
        return slots[index];
    }

    public bool IsMaxLevel(CardData card)
    {
        for(int i = 0; i<slots.Length; i++)
        {
            if (slots[i].card == card && slots[i].level >= MaxLevel)                //이 슬롯에 장착된 카드가 물어본 카드와 같은가? 빈슬롯은 card가 null이라 같지 않으니 걸러짐
            {
                return true;
            }
        }
        return false;
    }

    public float DamageBonus { get; private set; }
    public float MoveSpeedBonus { get; private set; }
    public float MaxHealthBonus { get; private set; }
    public float RangeBonus { get; private set; }
    public float AttackSpeedBonus { get; private set; }

    public enum SelectResult { Equipped, Merged, NeedReplace }
    public const int MaxLevel = 5;

    private void Awake()
    {
        slots = new CardSlot[SlotCount];                    //빈상자 4개를 만듦
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new CardSlot();              //반복문으로 자리마다 진짜 상자를 하나씩 만들어 넣기
        }
    }

    [ContextMenu("Fill Test slot 0")]
    private void FillTestSlot()
    {
        slots[0].card = testCard;
        slots[0].level = 1;
    }

    [ContextMenu("Print Slots")]
    private void PrintSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty)
            {
                Debug.Log($"슬롯 {i} : 비어있음");
            }
            else
            {
                Debug.Log($"슬롯  {i} : {slots[i].card.CardName} Lv {slots[i].level}");
            }
        }
    }

    public SelectResult TrySelect(CardData card)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].card == card)      //이 부분이 먼저 들어가야함.
            {
                if (slots[i].level < MaxLevel) slots[i].level++;
                Debug.Log($"{card.CardName} 합치기 Lv {slots[i].level}");
                Recalculate();
                return SelectResult.Merged;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i].card = card;
                slots[i].level = 1;
                Debug.Log($"{card.CardName} 장착 : 슬롯 {i}");
                Recalculate();
                return SelectResult.Equipped;
            }
        }
        return SelectResult.NeedReplace;
    }

    private void Recalculate()
    {
        //처음부터 다시 하기 위해서 초기값을 0으로 설정함. 카드가 바뀌었을때 대비용
        DamageBonus = 0;
        MoveSpeedBonus = 0;
        MaxHealthBonus = 0;
        RangeBonus = 0;
        AttackSpeedBonus = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty) continue;

            float amount = slots[i].card.Value * slots[i].level;

            switch (slots[i].card.Effect)
            {
                case CardEffect.Damage:
                    DamageBonus += amount;
                    break;
                case CardEffect.MoveSpeed:
                    MoveSpeedBonus += amount;
                    break;
                case CardEffect.MaxHealth:
                    MaxHealthBonus += amount;
                    break;
                case CardEffect.Range:
                    RangeBonus += amount;
                    break;
                case CardEffect.AttackSpeed:
                    AttackSpeedBonus += amount;
                    break;

            }
        }
        CountElements();
        DamageBonus += GetSynergyTier(Element.Fire) * 1;
        RangeBonus += GetSynergyTier(Element.Water) * 1;
        MaxHealthBonus += GetSynergyTier(Element.Earth) * 3;
        AttackSpeedBonus += GetSynergyTier(Element.Wind) * 0.5f;

        Debug.Log($"보너스 합계 : 공격력 {DamageBonus}, 이동속도 {MoveSpeedBonus}, 최대체력 {MaxHealthBonus}, 사거리 : {RangeBonus}, 공격속도 :{AttackSpeedBonus}");
        playerMove.SetMoveSpeedBonus(MoveSpeedBonus);
        playerAttack.SetDamageBonus(Mathf.RoundToInt(DamageBonus));
        autoTargeting.SetRangeBonus(RangeBonus);
        playerHealth.SetMaxHealthBonus(Mathf.RoundToInt(MaxHealthBonus));
        playerAttack.SetAttackSpeedBonus(AttackSpeedBonus);

        playerAttack.SetAttackElement(GetDominantElement());

        SlotsChanged?.Invoke();
    }

    private int GetSynergyTier(Element element)
    {
        int count = elementCounts[(int)element];
        if (count >= 3) return 2;
        if (count >= 2) return 1;
        return 0;
    }

    public void Replace(int slotIndex, CardData card)
    {
        slots[slotIndex].card = card;
        slots[slotIndex].level = 1;
        Debug.Log($"{card.CardName} 교체 : 슬롯 : {slotIndex}");
        Recalculate();
    }

    private Element GetDominantElement()
    {
        Element best = Element.None;
        int bestCount = 0;

        for(int i = 1; i<elementCounts.Length; i++)             //0은 None이니까 1번부터 돈다.
        {
            if (elementCounts[i]> bestCount)
            {
                bestCount = elementCounts[i];
                best = (Element)i;
            }
        }
        return best;
    }
}
