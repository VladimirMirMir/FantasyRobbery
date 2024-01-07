using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaxPlayersCount : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Slider _slider;

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        _text.text = _slider.value.ToString();
    }
}
