using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using System;

public class DataManage : MonoBehaviour
{
    static string persistentDataPath;
    static string folderName = "Data";

    public static DataManage instance;

    private void Awake()
    {
        persistentDataPath = Application.persistentDataPath;
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SaveTime()
    {
        string fileName = PlayerPrefs.GetString("cur_user", "user") + ".json";

        UserData newData = new UserData
        {
            date = System.DateTime.Now.ToString("dd/MM/yyyy"),
            time_turn_off_light = System.DateTime.Now.ToString("hh:mm"),
        };

        //Debug.Log(JsonUtility.ToJson(newData));
        

        if (!Directory.Exists(GetDirPath()))
        {
            Directory.CreateDirectory(GetDirPath());
        }

        

        //FileStream file;
        if (!File.Exists(GetFilePath(fileName)))
        {
            
            //File.Create(GetFilePath(fileName));
            DataSet newDataSet = new DataSet
            {
                data = new List<UserData>() { newData },
            };
            string json = JsonUtility.ToJson(newDataSet);
            
            var jsonObj = JObject.Parse(json);
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj,
                                   Newtonsoft.Json.Formatting.Indented);

            //string row = Js

            StreamWriter writer = new StreamWriter(GetFilePath(fileName));
            writer.WriteLine(newJsonResult);
            writer.Close();
            
        }
        else
        {
            string row = JsonUtility.ToJson(newData);

            var json = File.ReadAllText(GetFilePath(fileName));
            var jsonObj = JObject.Parse(json);
            var data = jsonObj.GetValue("data") as JArray;
            var item = JObject.Parse(row);

            string lastDate = data.Last["date"].ToString();
            string lastTime = data.Last["time_turn_off_light"].ToString();

            if (DateTime.Now.Subtract(DateTime.Parse(lastDate + " " + lastTime)).Minutes <= 9*60)
            {
                //Debug.Log("ok");
                data.RemoveAt(data.Count - 1);
            }
            data.Add(item);
            if (data.Count > 30)
            {
                data.RemoveAt(0);
            }
            

            jsonObj["data"] = data;
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj,
                                   Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(GetFilePath(fileName), newJsonResult);
        }

        //file.Close();
    }

    public List<UserData> GetData()
    {
        string fileName = PlayerPrefs.GetString("cur_user") + ".json";
        List<UserData> dataSet = new List<UserData>();

        var json = File.ReadAllText(GetFilePath(fileName));
        var jObject = JObject.Parse(json);

        if (jObject != null)
        {
            JArray data = (JArray)jObject["data"];
            if (data != null)
            {
                foreach (var row in data)
                {
                    UserData item = new UserData
                    {
                        date = row["date"].ToString(),
                        time_turn_off_light = row["time_turn_off_light"].ToString(),
                    };
                    dataSet.Add(item);
                }
            }
        }

        return dataSet;
    }
    /*
    public int setEndDay(string time)
    {

    }
    */

    static string GetFilePath(string fileName)
    {
        return GetDirPath() + "/" + fileName;
    }

    static string GetDirPath()
    {
        return persistentDataPath + "/" + folderName;
    }


}

[Serializable]
public class UserData
{
    public string date;
    public string time_turn_off_light;
}

[Serializable]
public class DataSet
{
    public List<UserData> data;
}
