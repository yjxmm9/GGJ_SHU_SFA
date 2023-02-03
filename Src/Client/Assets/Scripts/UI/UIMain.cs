using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{

    private void Start()
    {
        UIManager.Instance.Show<UIInit>(this.transform);
    }

    public void ShowTopUI()
    {
        UIManager.Instance.Show<UITop>(this.transform);
    }

}
