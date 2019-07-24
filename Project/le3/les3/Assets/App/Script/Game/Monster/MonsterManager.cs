using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Framework;
public class MonsterManager:MonoBehaviour
{
    private MonsterFacotry factory = null;

    private Transform[] spawnPoints = null;
    private void Awake()
    {
        factory =  gameObject.AddComponent<MonsterFacotry>();
    }

    public bool Init(Transform[] spawnPoints,Transform target)
    {
        this.spawnPoints = spawnPoints;

        var datatable =  LocalData.Instance.GetTable<MonsterTriggerData>();

        for(int i = 0; i < datatable.Count(); i++)
        {
            var trigger =  datatable.QueryIndex<MonsterTriggerData>(i);
            Timer timer = new Timer((agrs) => {
                int id = (int)agrs;
                DebugLog.LogFormat("id :{0}", id);
                int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
                GameObject monster =  factory.CreateMonster(id, spawnPoints[spawnPointIndex]);
                var moveComponent = monster.AddComponent<EnemyMovement>();
                monster.AddComponent<EnemyHealth>();
                moveComponent.Target(target);

            }, trigger.time, trigger.repeat, trigger.id);
            timer.Start();
        }
        return true;
    }

    private void OnDestroy()
    {
        TimerManager.Instance.Reset();
    }


    private void Update()
    {

    }




}

