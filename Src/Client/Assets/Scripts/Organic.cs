using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organic : MonoBehaviour
{
    public Collider myCollider;
    public MeshRenderer myRenderer;
    public AnimationCurve organicCurve;


    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Eat()
    {
        myCollider.enabled = false;
        StartCoroutine(EatRoutine());
    }

    IEnumerator EatRoutine()
    {
        Material organicMaterial = myRenderer.material;
        float organicContent = 1;
        while (organicContent > 0)
        {
            organicMaterial.color = new Color(organicMaterial.color.r, organicMaterial.color.g, organicMaterial.color.b, organicCurve.Evaluate(organicContent));
            organicContent -= Time.deltaTime;
            yield return null;
        }
    }
}
