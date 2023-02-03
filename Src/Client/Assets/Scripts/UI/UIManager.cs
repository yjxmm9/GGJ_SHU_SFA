using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    class UIElement
    {
        //prefab所在路径
        public string Resources;
        //Cache=true：关闭时不销毁游戏物体实例，而是隐藏；Cache=false：关闭时直接销毁游戏物体实例
        public bool Cache;
        //用于存储实例化的游戏物体实例
        public GameObject Instance;
    }

    //字典，管理UIElement
    private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();
    //构造函数，添加UITest进入字典
    public UIManager()
    {
        //将需要使用UI框架的类添加到管理字典中，才能执行UI框架中的逻辑
        //this.UIResources.Add(typeof(UITest), new UIElement() { Resources = "UI/UITest", Cache = true });
        this.UIResources.Add(typeof(UISkill), new UIElement() { Resources = "UI/UISkill", Cache = true });
    }

    ~UIManager()
    {

    }
    /// <summary>
    /// 显示UI逻辑，返回泛型T，为了便于其他类调用T类中的方法（如修改UITest中的标题等方法）
    /// </summary>
    public T Show<T>()
    {
        //播放音效
        //SoundManager.Instance.PlaySound("ui_open");
        Type type = typeof(T);
        //判断字典中是否有该类型
        if (this.UIResources.ContainsKey(type))
        {
            //从字典中取出对应的UIElement
            UIElement info = this.UIResources[type];
            //Instance不为空直接让他显示
            if (info.Instance != null)
            {
                info.Instance.SetActive(true);
            }
            //Instance为空，则加载prefab并且实例化赋给Instance
            else
            {
                //Resources文件夹中加载prefab
                UnityEngine.Object prefab = Resources.Load(info.Resources);
                if (prefab == null)
                {
                    return default(T);
                }
                //实例化prefab，并赋给Instance
                info.Instance = (GameObject)GameObject.Instantiate(prefab);
            }
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }
    /// <summary>
    /// 关闭UI逻辑
    /// </summary>
    /// <param name="type"></param>
    public void Close(Type type)
    {
        //播放音效
        //SoundManager.Instance.PlaySound("ui_close");
        //判断字典中是否包含对应的类型
        if (this.UIResources.ContainsKey(type))
        {
            //获取到对应的UIElement
            UIElement info = UIResources[type];
            //如果需要Cache，则隐藏Instance
            if (info.Cache)
            {
                info.Instance.SetActive(false);
            }
            //如果不需要Cache，则销毁游戏实例并清空Instance
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }
}
