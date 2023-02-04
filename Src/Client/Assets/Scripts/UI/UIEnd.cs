using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnd : UIWindow
{
    public override void OnCloseClick()
    {
        AudioManager.Instance.VictorSource.Stop();
        GM.Instance.cameraControler.EndRemake();
        base.OnCloseClick();
    }
}
