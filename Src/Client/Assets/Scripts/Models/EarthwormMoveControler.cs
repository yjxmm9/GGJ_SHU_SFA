﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthwormMoveControler : MonoSingleton<EarthwormMoveControler>
{
    public bool dead = true;
    public Slider HPSlider;
    public GameObject organicPrefab;
    //public GameObject headMesh;
    public LayerMask raycastMask;
    public float speed;//前进速度
    public float turnspeed = 0.5f;//转向速度
    public Vector2 gap;//两个身体节点之间的距离
    public GameObject bodyPrefab;
    public GameObject wormGameObject;
    public List<Transform> bodyList = new List<Transform>();

    private Vector3 InitPos;
    private Vector3 InitRot;
    private Quaternion headMeshRot;
    // Start is called before the first frame update
    protected override void OnStart()
    {
        gap = bodyList[0].localPosition - bodyList[1].localPosition;//初始化gap
        InitPos = bodyList[0].transform.position;
        InitRot = bodyList[0].eulerAngles;
        //headMeshRot = headMesh.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dead) { return; }
        //Move();
        Turn();
        Eat();
        LimitPosition();
        if (Input.GetKeyDown(KeyCode.E))
        {
            EarthwormGrow();
        }
        //headMesh.transform.rotation = bodyList[0].rotation * headMeshRot;

    }

    public void StartGame()
    {
        dead = false;
        //Debug.Log("remake");
        InitPositionsAndRotations(InitPos, InitRot);
        InitCountOfBody();
        HPSlider.value = 10;
        HPSlider.gameObject.SetActive(true);
    }

    void Move()
    {
        bodyList[0].transform.Translate(bodyList[0].InverseTransformDirection(-bodyList[0].up.normalized) * speed * Time.fixedDeltaTime);//头部一直前进
        Debug.Log(-bodyList[0].up);
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
        //RaycastHit hit;
        Collider[] hits;

        if ((hits = Physics.OverlapSphere(bodyList[0].position, 0.5f, raycastMask)).Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Poison poison = hits[i].GetComponent<Poison>();
                if (poison != null)
                {
                    //Debug.Log("11");
                    poison.Drink();
                    EarthwormGrow();
                    Vector3 organicPosition = bodyList[0].position + new Vector3(0.5f, -0.5f, 0.08f);
                    Instantiate(organicPrefab, organicPosition, organicPrefab.transform.rotation);
                    Move();
                }

                Oxygen oxygen = hits[i].GetComponent<Oxygen>();
                //Debug.Log(oxygen);
                if (oxygen != null)
                {
                    oxygen.Breath();
                    HPSlider.value += 3;
                    Move();
                    //Debug.Log(hit);
                }

                HardSoil hardSoil = hits[i].GetComponent<HardSoil>();
                if (hardSoil != null)
                {
                    hardSoil.Break();
                    Move();
                    AudioManager.Instance.SoilSFX();
                }
            }
        }
        else { Move(); }
    }

    void InitPositionsAndRotations(Vector3 targetPos, Vector3 targetEulerAngle)
    {
        bodyList[0].transform.position = targetPos;
        for (int i = 1; i < bodyList.Count; i++)
        {
            bodyList[i].localPosition = bodyList[0].localPosition + new Vector3(0, i, 0);
        }

        for (int i = 0; i < bodyList.Count; i++)
        {
            bodyList[i].eulerAngles = targetEulerAngle;
        }
    }

    void InitCountOfBody()
    {
        for (; bodyList.Count > 3;)
        {
            Destroy(bodyList[bodyList.Count - 1].gameObject);
            bodyList.Remove(bodyList[bodyList.Count - 1]);
        }
    }

    void LimitPosition()
    {
        if (bodyList[0].transform.position.y > -0.2)
        {
            InitPositionsAndRotations(InitPos, InitRot);
        }
        LineRenderer root = RootControler.Instance.root;
        if ((bodyList[0].transform.position - root.GetPosition(root.positionCount - 1)).y > 4.0 || (bodyList[0].transform.position - root.GetPosition(root.positionCount - 1)).y < -7.5)
        {
            InitPositionsAndRotations(root.GetPosition(root.positionCount - 1) + new Vector3(1, -1, 0), InitRot);
        }
    }
}