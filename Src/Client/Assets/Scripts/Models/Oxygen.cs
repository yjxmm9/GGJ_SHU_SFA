using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : MonoBehaviour
{
    public float oxygenAmount;
    public Collider myCollider;
    public MeshRenderer myRenderer;
    public AnimationCurve oxygenCurve;

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
        myCollider.enabled = false;
        //Debug.Log("iambreath");
        StartCoroutine(BreathRoutine());
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed);
    }

    IEnumerator BreathRoutine()
    {
        Material oxygenMaterial = myRenderer.material;
        float oxygenContent = 1;
        while (oxygenContent > 0)
        {
            oxygenMaterial.color = new Color(oxygenMaterial.color.r, oxygenMaterial.color.g, oxygenMaterial.color.b, oxygenCurve.Evaluate(oxygenContent));
            oxygenContent -= Time.deltaTime;
            yield return null;
        }
    }

}
