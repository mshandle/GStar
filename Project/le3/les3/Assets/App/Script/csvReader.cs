using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csvReader : MonoBehaviour
{
    [System.Serializable]
    public class csvConfig
    {
        public int id;
        public string name;
        public float value;
    }

    // Start is called before the first frame update
    void Start()
    {
        TextAsset text = Resources.Load<TextAsset>("Data/csv");
        var config =  CSVSerializer.Deserialize<csvConfig>(text.text);
        Debug.Log(config.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
