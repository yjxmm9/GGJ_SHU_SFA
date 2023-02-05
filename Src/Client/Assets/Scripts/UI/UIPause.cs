using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIPause : UIWindow
{
    public override void OnCloseClick()
    {
        Debug.Log("ClosePauseUI");
        UIMain.Instance.isPaused = false;
        UIManager.Instance.Close(Type.GetType("UIPause"));
        Time.timeScale = 1;
        base.OnCloseClick();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


}
