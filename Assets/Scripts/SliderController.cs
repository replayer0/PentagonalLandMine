using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderController : MonoBehaviour
{
    public UnityEngine.UI.Slider Slider;
    public UnityEngine.UI.Text Text;
    
    void Update()
    {
        if (null != Text)
        {
            Text.text = Slider.value.ToString();
        }
    } 

    public void SetValue(float value)
    {
        Slider.value = value;
    }

    public float GetValue()
    {
        return Slider.value;
    }

    public void SetMin(float value)
    {
        Slider.minValue = value;
    }

    public void SetMax(float value)
    {
        Slider.maxValue = value;
    }
}
