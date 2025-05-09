using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine.UI;
using UnityEngine;

[Serializable]
public class UI_InputFields
{
    public TextMeshProUGUI ResultText; // 결과 텍스트
    public TMP_InputField IDInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordComfirmInputField;
    public Button ConfirmButton;
}


public class UI_LoginScene : MonoBehaviour
{
    [Header("패널")]
    public GameObject LoginPanel;
    public GameObject RegisterPanel;

    [Header("로그인")] 
    public UI_InputFields LoginInputFields;


    [Header("회원가입")]
    public UI_InputFields RegisterInputFields;

    // 게임 시작하면 로그인 패널은 켜주고 회원가입은 꺼주고..
    private void Start()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }

    // 회원가입 버튼 클릭
    public void OnClickGotoRegisterButton()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    public void OnClickGotoLoginButton()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }

    // 회원가입
    public void Register()
    {
        // 1. 아이디 입력을 확인한다.
        string id = RegisterInputFields.IDInputField.text;
        if(string.IsNullOrEmpty(id))
        {
            RegisterInputFields.ResultText.text = "아이디를 입력해주세요.";
            return;
        }

        // 2. 비밀번호 입력을 확인한다.
        string password = RegisterInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            RegisterInputFields.ResultText.text = "비밀번호를 입력해주세요.";
            return;
        }

        // 3. 2차 비밀번호 입력을 확인하고, 1차 비밀번호 입력과 같은지 확인한다.
        string passwordComfirm = RegisterInputFields.PasswordComfirmInputField.text;
        if (password != passwordComfirm)
        {
            RegisterInputFields.ResultText.text = "비밀번호가 다릅니다.";
            return;
        }

        // 4. PlayerPrefabs를 이용해서 아이디와 비밀번호를 저장한다.
        PlayerPrefs.SetString(id, password);

        // 5. 로그인 창으로 돌아간다. (이때 아이디는 자동 입력되어 있다.)
        OnClickGotoLoginButton();
    }
}
