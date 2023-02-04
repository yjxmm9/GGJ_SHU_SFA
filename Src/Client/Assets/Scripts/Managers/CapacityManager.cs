using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Newtonsoft.Json;
using UnityEngine;

class CapacityManager:Singleton<CapacityManager>
{
    public string DataPath;
    public Dictionary<int, Dictionary<int,CapacityDefine>> Capacities = null;


    public int[] myPoints = new int[4];

    private string[] prefsName = new string[4]
    {
        "P0",
        "P1",
        "P2",
        "P3"
    };

    public int cash;

    public CapacityManager()
    {
        this.DataPath = "Data/";
        string json = File.ReadAllText(this.DataPath + "CapacityDefine.txt");
        this.Capacities = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, CapacityDefine>>>(json);
        myPoints[0] = PlayerPrefs.GetInt("P0", 0);
        myPoints[1] = PlayerPrefs.GetInt("P1", 0);
        myPoints[2] = PlayerPrefs.GetInt("P2", 0);
        myPoints[3] = PlayerPrefs.GetInt("P3", 0);
        cash = PlayerPrefs.GetInt("cash", 0);
        //cash = 100;
    }

    public void Init()
    {

    }

    public float GetSkillPoint(int capacityIdx)
    {
        var capacity = this.Capacities[capacityIdx + 1];
        CapacityDefine define;
        if (capacity.TryGetValue(myPoints[capacityIdx], out define))
        {
            return define.upgrade;
        }
        return 0;
    }

    public bool OnAddPoint(int capacityIdx,int pointIdx)
    {
        var capacity = this.Capacities[capacityIdx+1];
        CapacityDefine define;
        capacity.TryGetValue(pointIdx+1, out define);
        if (cash<define.cost)
        {
            return false;
        }
        cash -= define.cost;
        this.myPoints[capacityIdx]++;
        PlayerPrefs.SetInt(prefsName[capacityIdx], pointIdx);
        return true;
    }

    public void AddCash()
    {
        cash ++;
        PlayerPrefs.SetInt("cash", cash);
    }


}
