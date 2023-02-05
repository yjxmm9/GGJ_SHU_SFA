using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITop : UIWindow
{
    public Text cash;
    public Text distance;

    private void OnEnable()
    {
        cash.text = CapacityManager.Instance.cash.ToString();
        distance.text = (-(int)RootControler.Instance.depth).ToString();
    }

    private void Update()
    {
        distance.text = (-(int)RootControler.Instance.depth).ToString();
        cash.text = CapacityManager.Instance.cash.ToString();
    }
}
