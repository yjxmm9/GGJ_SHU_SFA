using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;//global  true:全局单例   false:场景单例
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType<T>();//场景中查找所有类型为T的引用
            }
            return instance;
        }

    }
    //单例初始化需要最先执行，因此使用awake，防止单例被赋值后再被初始化导致引用丢失
    void Awake()
    {
        if (global)
        {
            //如果该单例已经存在（不为空 且 与当前脚本不一样），则销毁自己，并不调用Awake逻辑
            if (instance != null && instance != this.gameObject.GetComponent<T>())
            {
                Destroy(this.gameObject);
                return;
            }
            //让这个游戏物体切换场景时不会被摧毁掉，做到全局单例
            DontDestroyOnLoad(this.gameObject);
            //防止其他地方没有调用过这个单例导致instance一直为空
            instance = this.gameObject.GetComponent<T>();
        }
        this.OnStart();
    }

    protected virtual void OnStart()
    {

    }
}
