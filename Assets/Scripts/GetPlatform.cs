using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlatform : MonoBehaviour
{
    [SerializeField] private GameObject PCWarningPanel;
    private void Start()
    {
        bool _isMobile = Application.isMobilePlatform;
        Debug.Log(_isMobile);
        if (_isMobile)
        {
            PCWarningPanel.SetActive(false);
        }
    }
}
