using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _markoPolo;

    void Start()
    {
        MarkoPolo();
    }

    void MarkoPolo()
    {
        string markoPolo = "";

        for(int i = 1; i <= 100; i++)
        {
            string line = i.ToString();
            if (i % 3 == 0 && i % 5 == 0) line += " MarkoPolo";
            else if (i % 3 == 0) line += " Marko";
            else if (i % 5 == 0) line += " Polo";

            markoPolo += line + "\n";
        }
        _markoPolo.text = markoPolo;
    }

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
