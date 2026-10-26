using UnityEngine;

public class ElementChart : MonoBehaviour
{
    public const float StrongMultiplier = 1.5f;
    public const float WeakMultiplier = 0.75f;

    //공격 속성이 방어속성에게 얼마나 먹히는가
    public static float GetMultiplier(Element attacker, Element defender)
    {
        if (attacker == Element.None || defender == Element.None) return 1f;
        if (Beats(attacker, defender)) return StrongMultiplier;
        if(Beats(defender, attacker)) return WeakMultiplier;

        return 1f;

    }

    //a가 b에게 강한가
    private static bool Beats(Element a, Element b)
    {
        if (a == Element.Water && b == Element.Fire) return true;
        if (a == Element.Fire && b == Element.Wind) return true;
        if (a == Element.Wind && b == Element.Earth) return true;
        if (a == Element.Earth && b == Element.Water) return true;

        return false;
    }
}
