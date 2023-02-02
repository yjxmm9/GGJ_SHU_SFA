using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthwormMoveControler : MonoBehaviour
{
    public float speed;
    public float turnspeed = 0.5f;
    public Vector2 gap;
    public Vector2 targetDir;
    public List<Transform> bodyList = new List<Transform>();
    // Start is called before the first frame update
    void Awake()
    {
        targetDir = bodyList[0].localPosition - bodyList[1].localPosition;
        gap = bodyList[0].localPosition - bodyList[1].localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        if (Input.GetKey(KeyCode.A))
        {
            bodyList[0].eulerAngles = bodyList[0].eulerAngles + new Vector3(0, 0, turnspeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            bodyList[0].eulerAngles = bodyList[0].eulerAngles + new Vector3(0, 0, -turnspeed);
        }
    }

    void Move()
    {
        bodyList[0].transform.Translate(-bodyList[0].up.normalized * speed * Time.fixedDeltaTime);
        for (int i = 1; i < bodyList.Count; i++)
        {
            if (gap.magnitude < (bodyList[i - 1].localPosition - bodyList[i].localPosition).magnitude)
            {
                bodyList[i].transform.Translate((bodyList[i - 1].localPosition - bodyList[i].localPosition).normalized * speed * Time.fixedDeltaTime);
            }
        }

    }
}
