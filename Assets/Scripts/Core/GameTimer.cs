using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float elapsed;
    private bool running = true;

    public float Elapsed => elapsed;
    private void Update()
    {
        if (!running) return;
        elapsed += Time.deltaTime;
    }

    public void Stop()
    {
        running = false;
    }
}
