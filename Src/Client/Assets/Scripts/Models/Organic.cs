using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organic : MonoBehaviour
{
    private void OnEnable()
    {
        GM.Instance.OnItemDestroy += ReStart;
    }

    public void ReStart()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GM.Instance.OnItemDestroy -= ReStart;
    }
    internal void Eat()
    {
        Destroy(gameObject);
    }
}
