using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private CharacterData[] characters;
    [SerializeField] private Outline[] cardOutlines;
    [SerializeField] private Animator[] cardAnimators;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button dailyButton;
    [SerializeField] private GameObject unlockButton;
    [SerializeField] private Color lockedTint = new Color(0.45f, 0.45f, 0.45f, 1f);

    private Image[] cardImages;
    private Image[] previewImages;
    private Color[] cardBaseColors;
    private Color[] previewBaseColors;
    private int selectedIndex = 0;

    private void Awake()
    {
        int n = characters.Length;
        cardImages = new Image[n];
        previewImages = new Image[n];
        cardBaseColors = new Color[n];
        previewBaseColors = new Color[n];

        for (int i = 0; i < n; i++)
        {
            cardImages[i] = cardOutlines[i].GetComponent<Image>();
            previewImages[i] = cardAnimators[i].GetComponent<Image>();
            cardBaseColors[i] = cardImages[i].color;
            previewBaseColors[i] = previewImages[i].color;
        }
    }

    private void OnEnable()
    {
        RefreshLocks();
        Select(selectedIndex);
    }

    private void RefreshLocks()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            bool unlocked = CharacterUnlocks.IsUnlocked(characters[i]);
            cardImages[i].color = unlocked ? cardBaseColors[i] : cardBaseColors[i] * lockedTint;
            previewImages[i].color = unlocked ? previewBaseColors[i] : previewBaseColors[i] * lockedTint;
        }
    }

    public void Select(int index)
    {
        selectedIndex = index;
        CharacterData c = characters[index];
        bool unlocked = CharacterUnlocks.IsUnlocked(c);

        if (unlocked) GameSession.selectedCharacter = c;
        playButton.interactable = unlocked;
        if(dailyButton != null) dailyButton.interactable = unlocked;

        // 잠긴 캐릭터일 때만 해금 버튼을 보여준다
        unlockButton.SetActive(!unlocked);
        if (!unlocked)
        {
            unlockButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{c.UnlockCost}코인으로 해금";
        }

        string stats = $"체력 {c.MaxHealth}              이동속도{c.MoveSpeed}               공격력 {c.BaseDamage}";
        if (unlocked)
        {
            infoText.text = $"{c.CharacterName}\n{c.Description}\n{stats}";
        }
        else
        {
            infoText.text = $"{c.CharacterName}\n잠겨 있습니다. {c.UnlockCost}코인이 필요합니다.\n{stats}";
        }

        for (int i = 0; i < cardOutlines.Length; i++)
        {
            cardOutlines[i].enabled = (i == index);
        }

        for (int i = 0; i < cardAnimators.Length; i++)
        {
            cardAnimators[i].SetBool("IsWalking", i == index);
        }
    }

    public void OnUnlockClicked()
    {
        CharacterData c = characters[selectedIndex];
        if (CharacterUnlocks.TryUnlock(c))
        {
            RefreshLocks();
            Select(selectedIndex);
        }
        else
        {
            infoText.text = $"{c.CharacterName}\n코인이 부족합니다. ({CoinWallet.Coins}/{c.UnlockCost})";
        }
    }
}