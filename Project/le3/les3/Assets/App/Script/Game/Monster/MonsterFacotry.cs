using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Framework;
using UnityEngine;
class MonsterFacotry : MonoBehaviour { 

    private ITable monsterDatas;
    public UnityEngine.GameObject CreateMonster(int id,Transform transform)
    {
        if(monsterDatas == null)
        {
            monsterDatas = LocalData.Instance.GetTable<monsterData>();
        }
        var data =  monsterDatas.Query<monsterData>(id);

        GameObject prefab = ResoruceMgr.Instance.Load<GameObject>(data.res);
        GameObject go = GameObject.Instantiate(prefab, transform.position,transform.rotation);
        go.layer = LayerMask.NameToLayer("Shootable");
        return go;
    }
}

