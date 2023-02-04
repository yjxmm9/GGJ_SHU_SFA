using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GM : MonoSingleton<GM>
{
    public Action OnItemDestroy;
    public RootControler rootControler;
    public CameraControler cameraControler;
    public EarthwormMoveControler earthWorm;
    //public UIManager ui;
    //public AudioManager sfx;

    public int cash;
    public int maxDepth;
    public float timer;
    public float maxTime;
    public bool newGamePlus;



    private void Update()
    {
        if (rootControler.dead || earthWorm.dead)
        {
            rootControler.dead = true;
            earthWorm.dead = true;
            AudioManager.Instance.growingSource.Stop();
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
