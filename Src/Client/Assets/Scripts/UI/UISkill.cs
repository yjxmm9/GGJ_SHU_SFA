using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkill : UIWindow
{



    public override void OnCloseClick()
    {
        RootControler.Instance.StartGame();
        EarthwormMoveControler.Instance.StartGame();
        AudioManager.Instance.growingSource.Play();
        UIMain.Instance.ShowTopUI();
        this.Close();
    }
}
