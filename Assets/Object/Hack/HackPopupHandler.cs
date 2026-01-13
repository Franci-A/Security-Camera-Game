using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackPopupHandler : MonoBehaviour
{
    [SerializeField] private Image[] icons;
    [SerializeField] private Sprite[] arrowSprites;

    public void InitPopup( List<ArrowNumber> sequence)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].sprite = arrowSprites[(int)sequence[i]];
        }
    }

    public void UpdatePopup(int index)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if(i < index)
            {
                icons[i].color = Color.gray;
            } else
                icons[i].color = Color.white;

        }
    }
}

public enum  ArrowNumber
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}