using UnityEngine;
using UnityEngine.UI;

public class UI_Firefly : MonoBehaviour
{
    Image[] fireflyImages = new Image[3];

    private void Awake()
    {
        int i = 0;
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            if (image.name != name)
            {
                fireflyImages[i] = image;
                image.color = new Color(1, 1, 1, .25f);
                i++;
            }
        }
    }

    public void UpdateUI(int newNrOfFireflies)
    {
        foreach (Image image in fireflyImages)
            image.color = new Color(1, 1, 1, .25f);

        int toBeAdded = 0;

        if (newNrOfFireflies <= fireflyImages.Length)
            toBeAdded = newNrOfFireflies;
        else
            toBeAdded = fireflyImages.Length;

        for (int i = 0; i < toBeAdded; i++)
            fireflyImages[i].color = new Color(1, 1, 1, 1);
    }
}
