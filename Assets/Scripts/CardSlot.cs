using System;

 [Serializable]
 public class CardSlot              //다른 스크립트 안에서 값으로 쓰는 클래스
{
    public CardData card;
    public int level;

    public bool IsEmpty => card == null;
}