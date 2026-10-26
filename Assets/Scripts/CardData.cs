
using UnityEngine;

[CreateAssetMenu(fileName ="NewCard",menuName = "Survival Hunt/Card Data")]
public class CardData :ScriptableObject     //에셋 파일이 됨
{
    [SerializeField] private string cardName;
    [SerializeField, TextArea] private string description;      //설명 문장용

    public string CardName => cardName;     //밖에서 읽을수 있고, 바꿀수는 없는것
    public string Description => description;
}