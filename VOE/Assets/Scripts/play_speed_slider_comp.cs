using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using voe;

public class play_speed_slider_comp : MonoBehaviour
{
    Slider my_slider;
    TextMeshProUGUI my_text;
    void Start()
    {
        my_slider = GetComponentInChildren<Slider>();
        my_text = GameObject.Find("CurrentEnemyDelayTime").GetComponent<TextMeshProUGUI>();
        SliderChanged();
    }
    
    public void SliderChanged()
    {
        float value = my_slider.value;
        value = (float)Math.Truncate(100.0f * value) / 100.0f;
        my_text.text = value.ToString() + "s";
        GameManager.set_enemy_delay_time(value);
    }
}
