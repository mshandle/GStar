using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class LocalData : BaseComponentTemplate<LocalData>
    {
        Dictionary<string, Table> datas = new Dictionary<string, Table>();
        public override bool Init(AppConfig config)
        {
            DebugLog.Log("LocalData");
            return base.Init(config);
        }

        public Table GetTable(string key)
        {
            Table table;
            if(datas.TryGetValue(key, out table))
            {
                return table;
            }
            return null;
        }

        public void AddTable(string key, Table table)
        {
            datas[key] = table;
        }

    }

}

