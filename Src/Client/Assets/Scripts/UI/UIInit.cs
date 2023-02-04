using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInit : UIWindow
{
    public Button startButton;

    
    // Start is called before the first frame update
    void OnEnable()
    {
        startButton.gameObject.SetActive(false);
        StartCoroutine("UIBeginAnim");
    }

    IEnumerator UIBeginAnim()
    {
        startButton.gameObject.SetActive(true);
        yield return null;
    }

    public override void OnCloseClick()
    {
        RootControler.Instance.StartGame();
        EarthwormMoveControler.Instance.StartGame();
        AudioManager.Instance.growingSource.Play();
        UIMain.Instance.ShowTopUI();
        LevelManager.Instance.Initialize();
        base.OnCloseClick();
    }
}
