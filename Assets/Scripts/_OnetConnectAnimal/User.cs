using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using TT;
using System.IO;

[System.Serializable]
public class UserData
{
    public int BestScore;
    public GridInfo Grid;
}

public class User : Singleton<User>
{
    public string Path => Application.streamingAssetsPath + "/user-data.json";

    public UserData Data { private set; get; }

    public User()
    {
        LoadData();
    }

    public void LoadData()
    {
        string data = File.ReadAllText(Path);
        Data = JsonUtility.FromJson<UserData>(data);
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(Data);

        File.WriteAllText(Path, data);
    }
}
