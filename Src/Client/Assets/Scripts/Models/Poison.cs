using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{

    public Collider myCollider;
    public MeshRenderer myRenderer;
    public AnimationCurve drinkCurve;


    public void Drink()
    {
        myCollider.enabled = false;
        StartCoroutine(DrinkRoutine());
    }

    IEnumerator DrinkRoutine()
    {
        Material PoisonMaterial = myRenderer.material;
        float waterContent = 1;
        while (waterContent > 0)
        {
            //Debug.Log(drinkCurve.Evaluate(waterContent));
            PoisonMaterial.color = new Color(PoisonMaterial.color.r, PoisonMaterial.color.g, PoisonMaterial.color.b, drinkCurve.Evaluate(waterContent));
            waterContent -= Time.deltaTime;
            yield return null;
        }
    }
}
