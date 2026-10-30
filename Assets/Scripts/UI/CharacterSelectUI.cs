using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private CharacterData[] characters;
    [SerializeField] private Outline[] cardOutlines;
    [SerializeField] private Animator[] cardAnimators;
    [SerializeField] private TextMeshProUGUI infoText;

    private int selectedIndex = 0;

    private void OnEnable()
    {
        Select(selectedIndex);
    }

    public void Select(int index)
    {
        selectedIndex = index;
        CharacterData c = characters[index];
        GameSession.selectedCharacter = c;

        infoText.text = $"{c.CharacterName}\n{c.Description}\n체력 {c.MaxHealth}              이동속도{c.MoveSpeed}               공격력 {c.BaseDamage}";

        for(int i = 0; i < cardOutlines.Length; i++)
        {
            cardOutlines[i].enabled = (i == index);
        }

        for(int i = 0; i<cardAnimators.Length; i++)
        {
            cardAnimators[i].SetBool("IsWalking", i == index);
        }
    }
}
