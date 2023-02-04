using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInit : UIWindow
{
    public Button startButton;
    public Button tutorialButton;
    public Text title;


    // Start is called before the first frame update
    void OnEnable()
    {
        startButton.gameObject.SetActive(false);
        tutorialButton.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        StartCoroutine("UIBeginAnim");
    }

    IEnumerator UIBeginAnim()
    {
        yield return new WaitForSeconds(1);
        title.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        startButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        tutorialButton.gameObject.SetActive(true);
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

    public void TutorialShow()
    {
        UIManager.Instance.Show<UITutorial>();
        Debug.Log("Show Tutorial");
    }
}
