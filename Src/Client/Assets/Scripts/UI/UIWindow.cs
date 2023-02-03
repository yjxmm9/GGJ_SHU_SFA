using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIWindow : MonoBehaviour
{
    public delegate void CloseHandler(UIWindow sender, WindowResult result);
    public event CloseHandler OnClose;

    public virtual Type Type { get { return this.GetType(); } }

    public enum WindowResult
    {
        None = 0,
        Yes,
        No,
    }
    //关闭UI的逻辑
    public void Close(WindowResult result = WindowResult.None)
    {
        //调用UIManager中的方法关闭
        UIManager.Instance.Close(this.Type);
        //触发关闭事件，用于在关闭后执行一些逻辑
        if (this.OnClose != null)
        {
            this.OnClose(this, result);
        }
        this.OnClose = null;
    }
    //关闭按钮的方法，可以重写
    public virtual void OnCloseClick()
    {
        this.Close();
    }
    //确认按钮的方法，可以重写
    public virtual void OnYesClick()
    {
        this.Close(WindowResult.Yes);
    }
    public virtual void OnNoClick()
    {
        this.Close(WindowResult.No);
    }
}
