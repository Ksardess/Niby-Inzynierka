using UnityEngine;
using UnityEngine.UI;

public class HelthBar : MonoBehaviour
{

    public Slider slider;

    public void SetHelth(int helth)
    {
        slider.value = helth;
    }
    
    public void SetMaxHelth(int helth)
    {
        slider.maxValue = helth;
        slider.value = helth;
    }
}
