using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillItem : MonoBehaviour
{
    public int index;
    public List<Image> skillPoints;
    public Sprite getPoint;
    public Sprite normalPoint;
    private int pointIndex=0;
    public int PointIndex
    {
        get
        {
            return pointIndex;
        }
        set
        {
            pointIndex = value;

            for (int i = 0; i < skillPoints.Count; i++)
            {
                skillPoints[i].overrideSprite = value >= i+1 ? getPoint : normalPoint;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        this.pointIndex = CapacityManager.Instance.myPoints[index];
    }

    public void OnPointAdd()
    {

        if (CapacityManager.Instance.OnAddPoint(index, pointIndex))
        {
            this.PointIndex++;
        }

    }
}
