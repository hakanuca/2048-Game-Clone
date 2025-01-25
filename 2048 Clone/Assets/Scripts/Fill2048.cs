using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fill2048 : MonoBehaviour
{
    public int value;
    [SerializeField] Text valueDisplay;

    public void FillValueUpdate(int valueIn)
    {
        value = valueIn;
        valueDisplay.text = value.ToString();
    }
}
