using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

using Newtonsoft;
using System;
[Serializable]
public class Server
{
    public string name;
    public string ip;

}
[Serializable]
public class Config
{
    public int iggid;
    public string language;
    public Dictionary<string, string> attributes;
    public List<Server> ServerList;
}


public class JsonReader : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
        TextAsset text = Resources.Load<TextAsset>("Data/json2");
        Config config =  JsonUtility.FromJson<Config>(text.text);
        Debug.Log(config.ServerList.Count);

        Config newJson =  Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(text.text);
        Debug.Log(config.ServerList.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
