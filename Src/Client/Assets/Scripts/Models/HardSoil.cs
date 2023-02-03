using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardSoil : MonoBehaviour
{
    public Collider myCollider;
    public MeshRenderer myRenderer;
    public AnimationCurve drinkCurve;


    public void Break()
    {
        myCollider.enabled = false;
        StartCoroutine(BreakRoutine());
    }

    IEnumerator BreakRoutine()
    {
        Material HardSoilMaterial = myRenderer.material;
        float SoilContent = 1;
        while (SoilContent > 0)
        {
            //Debug.Log(drinkCurve.Evaluate(waterContent));
            HardSoilMaterial.color = new Color(HardSoilMaterial.color.r, HardSoilMaterial.color.g, HardSoilMaterial.color.b, drinkCurve.Evaluate(SoilContent));
            SoilContent -= Time.deltaTime;
            yield return null;
        }
    }
}
