using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float waterAmount;
    public Collider myCollider;
    public MeshRenderer myRenderer;
    public AnimationCurve drinkCurve;

    private void OnEnable()
    {
        waterAmount = transform.localScale.magnitude / 30f;
    }
    public void Drink()
    {
        myCollider.enabled = false;
        StartCoroutine(DrinkRoutine());
    }

    IEnumerator DrinkRoutine()
    {
        Material[] waterMaterials = myRenderer.materials;
        Debug.Log(waterMaterials.Length);
        float waterContent = 1;
        while (waterContent > 0)
        {
            //Debug.Log(drinkCurve.Evaluate(waterContent));
            foreach (var material in waterMaterials)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, drinkCurve.Evaluate(waterContent));
            }
            waterContent -= Time.deltaTime;
            yield return null;
        }
    }
}
