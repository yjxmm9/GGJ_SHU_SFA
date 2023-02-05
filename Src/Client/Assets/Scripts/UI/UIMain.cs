using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    public bool isPaused = false;

    private void Start()
    {
        UIManager.Instance.Show<UIInit>();
    }

    public void ShowTopUI()
    {
        UIManager.Instance.Show<UITop>(this.transform);
    }

    public void CloseTopUI()
    {
        UIManager.Instance.Close(Type.GetType("UITop"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            isPaused = true;
            ShowPauseUI();
        }

    }

    public void ShowPauseUI()
    {
        Time.timeScale = 0;
        UIManager.Instance.Show<UIPause>();
    }



}
