using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MultiTextUI : MonoBehaviour
{
    [SerializeField] private string ruText;
    [SerializeField] private string enText;

    private TMP_Text _text;

    public static string lang;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        if (_text != null)
        {
            _text.text = lang == "ru" ? ruText : enText;
        }
        else
        {
            GetComponent<Text>().text = lang == "ru" ? ruText : enText;
        }
    }
}