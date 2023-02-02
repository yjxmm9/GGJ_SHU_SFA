using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthwormMoveControler : MonoBehaviour
{
    public float speed;//前进速度
    public float turnspeed = 0.5f;//转向速度
    public Vector2 gap;//两个身体节点之间的距离
    public GameObject bodyPrefab;
    public GameObject wormGameObject;
    public List<Transform> bodyList = new List<Transform>();
    // Start is called before the first frame update
    void Awake()
    {
        gap = bodyList[0].localPosition - bodyList[1].localPosition;//初始化gap
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Turn();
        EarthwormGrow();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject body = Instantiate(bodyPrefab, wormGameObject.transform);
            body.transform.localPosition = bodyList[bodyList.Count - 1].localPosition - (bodyList[bodyList.Count - 2].localPosition - bodyList[bodyList.Count - 1].localPosition).normalized;
            bodyList.Add(body.transform);
        }
    }
}
