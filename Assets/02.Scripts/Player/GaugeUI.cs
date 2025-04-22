using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour
{
    public Slider Slider;

    public float Ratio;

    public void UpdateValue()
    {
        Slider.value = Ratio;
    }
}
