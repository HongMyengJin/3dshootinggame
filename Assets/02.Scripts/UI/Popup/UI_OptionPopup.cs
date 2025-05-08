using Unity.VisualScripting;
using UnityEngine;

public class UI_OptionPopup : UI_Popup
{

    public void OnClickContinueButton()
    {
        GameManager_.Instance.Continue();
    }

    public void OnClickRetryButton()
    {
        gameObject.SetActive(false);
        GameManager_.Instance.Restart();
    }

    public void OnClickQuitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.quit();
        #endif
    }

    public void OnClickCreditButton()
    {
        PopupManager.Instance.Open(EPopupType.UI_CreditPopup);
        GameManager_.Instance.Credit();
    }
}