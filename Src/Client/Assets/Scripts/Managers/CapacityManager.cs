using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Newtonsoft.Json;

class CapacityManager:Singleton<CapacityManager>
{
    public string DataPath;
    public Dictionary<int, Dictionary<int,CapacityDefine>> Capacities = null;

    public CapacityManager()
    {
        this.DataPath = "Data/";
        string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
        this.Capacities = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, CapacityDefine>>>(json);
    }


}
