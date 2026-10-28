using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    [SerializeField] private GateWave wave;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMove>() == null) return;
        wave.StartWave();
    }
}
