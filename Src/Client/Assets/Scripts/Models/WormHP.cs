using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WormHP : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider HPSlider;
    public Image HPFillImage;//血条
    //public Image HPBackgroundImage;//血条背景
    public float HPDecreaseSpeed = 1;
    public GameObject earthWormHeadGameObject;
    public Camera _camera;
    void Awake()
    {
        //Debug.Log(HPFillImage.color);
        HPFillImage.color = new Color(0.252f, 0.499f, 0.877f, 1.000f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HPPosUpdate();
        if (earthWormHeadGameObject.GetComponent<EarthwormMoveControler>().dead) return;
        HPValueUpdate();
        
    }

    void HPValueUpdate()
    {
        HPSlider.value = HPSlider.value - HPDecreaseSpeed / 50;//每秒损失

        if (HPSlider.value < 3)//HP低于3时变红
        {
            HPFillImage.color = new Color(0.722f, 0.052f, 0.066f, 1.000f);
        }
        else { HPFillImage.color = new Color(0.252f, 0.499f, 0.877f, 1.000f); }

        if (Input.GetKeyDown(KeyCode.E))//接触氧气时，HP增加
        {
            HPSlider.value = HPSlider.value + 3;
        }
    }
    void HPPosUpdate()
    {
        var headPos = _camera.WorldToScreenPoint(earthWormHeadGameObject.transform.GetChild(0).transform.position);
        HPFillImage.rectTransform.position = new Vector3(headPos.x + 35f, headPos.y, headPos.z);
    }
}
