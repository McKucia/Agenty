using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarkoPolo : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] TextMeshProUGUI _buttonText;
    [SerializeField] GameObject _markoPoloPanel;
    [SerializeField] TextMeshProUGUI _markoPoloTextContainer;

    bool isActive = false;

    void Start()
    {
        CalculateMarkoPolo();
        _button.onClick.AddListener(DisplayMarkoPolo);
    }

    void CalculateMarkoPolo()
    {
        string markoPolo = "";

        for (int i = 1; i <= 100; i++)
        {
            string line = i.ToString();
            if (i % 3 == 0 && i % 5 == 0) line += " MarkoPolo";
            else if (i % 3 == 0) line += " Marko";
            else if (i % 5 == 0) line += " Polo";

            markoPolo += line + "\n";
        }
        _markoPoloTextContainer.text = markoPolo;
    }

    void DisplayMarkoPolo()
    {
        isActive = !isActive;
        _markoPoloPanel.SetActive(isActive);
        _buttonText.text = isActive ? "Hide" : "MarkoPolo";
    }
}
