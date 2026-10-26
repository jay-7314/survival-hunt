using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CardPicker : MonoBehaviour
{
    [SerializeField] private List<CardData> allCards;       //전체목록의 복사본 만들기

    public List<CardData> PickRandom(int count)
    {
        List<CardData> candidates = new List<CardData>(allCards);
        List<CardData> picked = new List<CardData>();

        for (int i = 0; i < count && candidates.Count > 0; i++)
        {
            int index = Random.Range(0, candidates.Count);
            picked.Add(candidates[index]);
            candidates.RemoveAt(index);
        }
        return picked;
    }
}
