using UnityEngine;

[CreateAssetMenu(menuName = "Survival Hunt/Character Data")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField, TextArea] private string description; 
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private int baseDamage = 1;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private CardData startingCard;

    public string CharacterName => characterName;
    public string Description => description;
    public float MoveSpeed => moveSpeed;
    public int BaseDamage => baseDamage;
    public int MaxHealth => maxHealth;
    public float AttackCooldown => attackCooldown;
    public CardData StartingCard => startingCard;
}