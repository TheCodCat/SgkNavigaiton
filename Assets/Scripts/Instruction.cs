using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instruction : MonoBehaviour
{
    private const string AUTO_KEY = "auto";
    [SerializeField] private GameObject _panel;
    [SerializeField] private Toggle _toggle;

    private void Start()
    {
        if (PlayerPrefs.HasKey(AUTO_KEY))
        {
            bool _auto = bool.Parse(PlayerPrefs.GetString(AUTO_KEY));
            Debug.Log(_auto);
            _toggle.isOn = _auto;
            
            if (_toggle.isOn)
            {
                _panel.SetActive(true);
            }
            else 
                _panel.SetActive(false);
        }
    }
    public void OpenCloseInstruction()
    {
        _panel.SetActive(!_panel.activeSelf);
    }
    public void SaveStateAuto(bool auto)
    {
        Debug.Log(auto);
        PlayerPrefs.SetString(AUTO_KEY, auto.ToString());
    }
}
