using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : MonoBehaviour
{
    public float oxygenAmount;

    public float lerpSpeed;
    public Vector3 targetPos;
    public float moveDis;

    private void OnEnable()
    {
        oxygenAmount = 5;
        targetPos = transform.position + Vector3.down * moveDis;
        GM.Instance.OnItemDestroy += ReStart;
    }

    public void ReStart()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (GM.Instance.OnItemDestroy != null)
            GM.Instance.OnItemDestroy -= ReStart;
    }
    public void Breath()
    {
        Destroy(gameObject);
        //Debug.Log("iambreath");
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed);
    }

}
