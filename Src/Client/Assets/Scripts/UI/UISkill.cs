using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UISkill : UIWindow
{



    public override void OnCloseClick()
    {
        //if (GM.Instance.OnItemDestroy != null)
        //{
        //    GM.Instance.OnItemDestroy();
        //}
        RootControler.Instance.StartGame();
        EarthwormMoveControler.Instance.StartGame();
        AudioManager.Instance.growingSource.Play();
        AudioManager.Instance.MusicBack();
        UIMain.Instance.ShowTopUI();
        LevelManager.Instance.Initialize();
        base.OnCloseClick();
    }
}
