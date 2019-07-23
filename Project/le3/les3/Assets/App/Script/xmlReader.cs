using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class xmlReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextAsset text = Resources.Load<TextAsset>("Data/xml");
        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.LoadXml(text.text);
        var root = XmlDoc.SelectSingleNode("config");
        var iggid = root.SelectSingleNode("iggid").InnerText;
        var language = root.SelectSingleNode("language").InnerText;
        var servers = root.SelectSingleNode("serverList");

        foreach (XmlNode item in servers.ChildNodes)
        {
            var name = item.Attributes["name"].InnerText;
            var ip = item.Attributes["ip"].InnerText;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
