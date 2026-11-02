using UnityEngine;

public static class ElementColors
{
    public static Color Get(Element element)
    {
        switch (element)
        {
            case Element.Fire: return new Color32(232, 69, 44, 255);
            case Element.Water: return new Color32(58, 110, 230, 255);
            case Element.Earth: return new Color32(186, 134, 62, 255);
            case Element.Wind: return new Color32(79, 208, 192, 255);
            default: return new Color32(120, 120, 120, 255);
        }
    }
}