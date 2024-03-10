using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
public class ResourceBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(int max) {
        slider.maxValue = max;
        slider.value = max;
    }
    public void SetMaxValue(int max, int val) {
        slider.maxValue = max;
        slider.value = val;
    }

    public void SetValue(int val) {
        slider.value = val;
    }
}
