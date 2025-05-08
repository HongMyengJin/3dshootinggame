using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EPopupType
{ 
    UI_OptionPopup,
    UI_CreditPopup,
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [Header("팝업 UI 참조")]
    public List<UI_Popup> Popups; // 모든 팝업을 관리하는데
    private List<UI_Popup> _openedPopups = new();
    private void Awake()
    {
        Instance = this;
    }

    public void Open(EPopupType type, Action closeCallback = null)
    {
        Open(type.ToString(), closeCallback);
    }

    public void Open(string popupName, Action closeCallback)
    {
        foreach(UI_Popup popup in Popups)
        {
            if (popup.gameObject.name == popupName)
            {
                popup.Open(closeCallback);
                _openedPopups.Add(popup);
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_openedPopups.Count > 0)
            {
                _openedPopups[_openedPopups.Count - 1].Close();
                _openedPopups.RemoveAt(_openedPopups.Count - 1);
            }
            else
                GameManager_.Instance.Pause();
        }
    }
}
