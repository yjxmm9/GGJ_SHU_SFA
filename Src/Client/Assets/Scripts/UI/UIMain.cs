using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{

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

}
