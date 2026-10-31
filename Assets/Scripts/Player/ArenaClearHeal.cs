using Unity.VisualScripting;
using UnityEngine;

public class ArenaClearHeal : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private int healAmount = 5;

    private void OnEnable()
    {
        GateWave.Cleared += HandleCleared;
    }

    private void OnDisable()
    {
        GateWave.Cleared -= HandleCleared;
    }

    private void HandleCleared()
    {
        health.Heal(healAmount);
    }
}
