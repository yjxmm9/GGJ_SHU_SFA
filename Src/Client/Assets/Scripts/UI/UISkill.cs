using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UISkill : UIWindow
{
    public Text cash;

    private void OnEnable()
    {
    }

    private void Update()
    {
        if (this.cash != null) this.cash.text = CapacityManager.Instance.cash.ToString();
    }



    public override void OnCloseClick()
    {
        if (GM.Instance.OnItemDestroy != null)
        {
            GM.Instance.OnItemDestroy();
        }
        RootControler.Instance.StartGame();
        EarthwormMoveControler.Instance.StartGame();
        AudioManager.Instance.growingSource.Play();
        AudioManager.Instance.MusicBack();
        UIMain.Instance.ShowTopUI();
        LevelManager.Instance.Initialize();
        base.OnCloseClick();
    }
}
