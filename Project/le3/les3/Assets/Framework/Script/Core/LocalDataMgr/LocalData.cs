using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class LocalData : BaseComponentTemplate<LocalData>
    {
        Dictionary<string, ITable> datas = new Dictionary<string, ITable>();
        public override bool Init(AppConfig config)
        {
            DebugLog.Log("LocalData");
            return base.Init(config);
        }

        public ITable GetTable<T>() where T : IRecord
        {
            System.Type type = typeof(T);
            string name = type.Name;

            ITable table;
            if (datas.TryGetValue(name, out table))
            {
                return table;
            }
            return null;
        }

        public void AddTable<T>(string path) where T : IRecord
        {
            //datas[key] = table;
            System.Type type = typeof(T);
            string name = type.Name;

            TextAsset textAsset = ResoruceMgr.instance.Load<TextAsset>(path);
            T[] records = CSVSerializer.Deserialize<T>(textAsset.text);

            Table table = new Table();
            table.AddRecords<T>(records);
            datas.Add(name, table);
        }

    }

}

