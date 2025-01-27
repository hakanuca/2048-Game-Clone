using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fill2048 : MonoBehaviour
{
    public int value;
    [SerializeField] Text valueDisplay;
    [SerializeField] float speed;
    bool hasCombine;

    Image myImage;
    public void FillValueUpdate(int valueIn)
    {
        value = valueIn;
        valueDisplay.text = value.ToString();

        // Yazı rengini ayarla
        UpdateTextColor(value);

        int colorIndex = GetColorIndex(value);
        Debug.Log(colorIndex);

        myImage = GetComponent<Image>();
        Color color = GameController2048.instance.fillColors[colorIndex];
        color.a = 1f; // Ensure alpha is fully opaque
        myImage.color = color;
    }

    private void Update()
    {
        if (transform.localPosition != Vector3.zero)
        {
            hasCombine = false;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        }
        else if (hasCombine == false)
        {
            if (transform.parent.GetChild(0) != this.transform)
            {
                Destroy(transform.parent.GetChild(0).gameObject);
            }
            hasCombine = true;
        }
    }
    public void Double()
    {
        value *= 2;
        GameController2048.instance.ScoreUpdate(value);
        valueDisplay.text = value.ToString();

        // Yazı rengini ayarla
        UpdateTextColor(value);

        int colorIndex = GetColorIndex(value);
        Debug.Log(colorIndex);

        Color color = GameController2048.instance.fillColors[colorIndex];
        color.a = 1f; // Ensure alpha is fully opaque
        myImage.color = color;

        GameController2048.instance.WinningCheck(value);
    }

    void UpdateTextColor(int value)
    {
        if (value == 2 || value == 4)
        {
            valueDisplay.color = Color.black; // Yazı rengini siyah yap
        }
        else
        {
            valueDisplay.color = Color.white; // Yazı rengini beyaz yap
        }
    }

    int GetColorIndex(int valueIn)
    {
        int index = 0;
        while (valueIn != 1)
        {
            index++;
            valueIn = valueIn / 2;
        }

        index--;
        return index;
    }
}
