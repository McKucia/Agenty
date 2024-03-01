using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _name;

    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;
    }

    public void SetHealth(int health)
    {
        if (health != (int)_slider.value)
            _slider.value = health;
    }

    public void SetActive(bool active)
    {
        if(active && gameObject.activeSelf) return;
        if(!active && !gameObject.activeSelf) return;

        gameObject.SetActive(active);
    }

    public void SetName(string name)
    {
        _name.text = name;
    }
}
