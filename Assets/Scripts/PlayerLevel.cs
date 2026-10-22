using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private int expToNextLevel = 5;

    private int level = 1;
    private int currentExp;
    public event Action<int> LeveledUp;     //객체 하나하나 자기채널을 가짐.


    private void OnEnable()
    {
        Enemy.Killed += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        Enemy.Killed -= HandleEnemyKilled;      //방송을 시작했으면 취소해야함.
    }

    private void HandleEnemyKilled(Enemy enemy)
    {
        AddExp(enemy.ExpReward);
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log($"경험치 : {currentExp} / {expToNextLevel}");

        while (currentExp >= expToNextLevel)        //경험치를 한번에 많이 얻으면 레벨이 두번이상 오를수 있어서 while로 설정
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentExp -= expToNextLevel;
        level++;
        expToNextLevel += 3;
        Debug.Log($"레벨업 ! 현재레벨 : {level}");
        LeveledUp?.Invoke(level);
    }

    [ContextMenu("Add 3 Exp")]
    private void TextAddExp()
    {
        AddExp(3);
    }

}
