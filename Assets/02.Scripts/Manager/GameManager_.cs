using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_ : MonoBehaviour
{
    private static GameManager_ _instance;
    public static GameManager_ Instance => _instance;
    private void Awake()
    {
        _instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
     
    // Update is called once per frame
    void Update()
    {
    }
    
    public void Pause()
    {
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;

        PopupManager.Instance.Open(EPopupType.UI_OptionPopup, closeCallback:Continue);
    }

    public void Continue()
    {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Restart()
    {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void Credit()
    {
        PopupManager.Instance.Open(EPopupType.UI_CreditPopup);
    }
}
