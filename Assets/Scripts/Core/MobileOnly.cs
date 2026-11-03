using UnityEngine;

public class MobileOnly : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(Application.isMobilePlatform);
    }
}