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
    private Stack<UI_Popup> _openedPopups = new();
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
                _openedPopups.Push(popup);
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
                while (true)
                {
                    UI_Popup popup = _openedPopups.Pop();

                    bool opened = popup.isActiveAndEnabled;
                    popup.Close();

                    if (opened || _openedPopups.Peek() == null) // 열려있는 팝업을 닫았거나 || 더 이상 닫을 팝업이 없으면 탈출!
                    {
                        break;
                    }
                }
            }
            else
            {
                GameManager_.Instance.Pause();
            }
        }
    }
}
