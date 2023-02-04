using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UISkill : UIWindow
{
    public Text cash;
    public int capCash;

    private void OnEnable()
    {
        capCash= CapacityManager.Instance.cash;
    }

    private void Update()
    {
        if (this.cash != null) this.cash.text = capCash.ToString();
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
