using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthwormMoveControler : MonoSingleton<EarthwormMoveControler>
{
    public bool dead = true;
    public Slider HPSlider;
    public GameObject poisonPrefab;
    public LayerMask raycastMask;
    public float speed;//前进速度
    public float turnspeed = 0.5f;//转向速度
    public Vector2 gap;//两个身体节点之间的距离
    public GameObject bodyPrefab;
    public GameObject wormGameObject;
    public List<Transform> bodyList = new List<Transform>();
    // Start is called before the first frame update
    protected override void OnStart()
    {
        gap = bodyList[0].localPosition - bodyList[1].localPosition;//初始化gap
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dead) { return; }
        Move();
        Turn();
        Eat();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EarthwormGrow();
        }

    }

    public void StartGame()
    {
        dead = false;
        HPSlider.gameObject.SetActive(true);
    }

    void Move()
    {
        bodyList[0].transform.Translate(-bodyList[0].up.normalized * speed * Time.fixedDeltaTime);//头部一直前进
        for (int i = 1; i < bodyList.Count; i++)//身体跟随头部前进
        {
            if (gap.magnitude < (bodyList[i - 1].localPosition - bodyList[i].localPosition).magnitude)//当两个身体节点过近时停止，否则前进
            {
                bodyList[i].transform.Translate((bodyList[i - 1].localPosition - bodyList[i].localPosition).normalized * speed * Time.fixedDeltaTime);
                //bodyList[i].eulerAngles = Quaternion.LookRotation((bodyList[i - 1].localPosition - bodyList[i].localPosition).normalized).eulerAngles;
            }
        }

    }
    void Turn()
    {
        if (Input.GetKey(KeyCode.A))
        {
            bodyList[0].eulerAngles = bodyList[0].eulerAngles + new Vector3(0, 0, turnspeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            bodyList[0].eulerAngles = bodyList[0].eulerAngles + new Vector3(0, 0, -turnspeed);
        }

    }

    void EarthwormGrow()
    {
        GameObject body = Instantiate(bodyPrefab, wormGameObject.transform);
        body.transform.localPosition = bodyList[bodyList.Count - 1].localPosition - (bodyList[bodyList.Count - 2].localPosition - bodyList[bodyList.Count - 1].localPosition).normalized;
        bodyList.Add(body.transform);
    }

    void Eat()
    {
        RaycastHit hit;

        if (Physics.Raycast(bodyList[0].position, -bodyList[0].up, out hit, 0.1f, raycastMask))
        {
            Poison poison = hit.collider.GetComponent<Poison>();
            if (poison != null)
            {
                //Debug.Log("11");
                poison.Drink();
                EarthwormGrow();
                Vector3 poisonPosition = bodyList[0].position + new Vector3(0.5f, -0.5f, 0.08f);
                Instantiate(poisonPrefab, poisonPosition, poison.transform.rotation);
            }

            Oxygen oxygen = hit.collider.GetComponent<Oxygen>();
            //Debug.Log(oxygen);
            if (oxygen != null)
            {
                oxygen.Breath();
                HPSlider.value += 3;
                //Debug.Log(hit);
            }
        }
    }
}