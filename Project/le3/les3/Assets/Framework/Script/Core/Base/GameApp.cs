using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public abstract class GameApp :MonoBehaviour
    {
        public virtual void Awake()
        {
            GameObject.DontDestroyOnLoad(this);
        }

        protected List<BaseComponent> components = new List<BaseComponent>();

        public bool InitFrameWork(AppConfig config)
        {
            AddGameComponent<LocalData>();
            AddGameComponent<ResoruceMgr>();
            AddGameComponent<NetMgr>();
            AddGameComponent<SceneMgr>();
            AddGameComponent<UIMgr>();
            AddGameComponent<TimerManager>();
            return this.InitComponents(config);


        }

        private T AddGameComponent<T>() where T : BaseComponentTemplate<T>
        {
            T component =  this.gameObject.AddComponent<T>();
            
            Type type = typeof(BaseComponentTemplate<>).MakeGenericType(typeof(T));
            var method =  type.GetMethod("RegisterComponent",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic);
            if (method != null)
            {
                method.Invoke(null,new object[] { component });
            }
            
            components.Add(component);

            /*
            string namekey = type.ToString();
            if (components.ContainsKey(namekey))
                return (T)components[namekey];
            components.Add(namekey, component);
             */
            return component;
        }

        
       /* public T GetComponent<T>() where T : BaseComponentTemplate<T>
        {
            Type type = typeof(BaseComponentTemplate<>).MakeGenericType(typeof(T));
            string namekey = type.ToString();

            if (components.ContainsKey(namekey))
                return (T)components[namekey];
            return null;
        }*/
        

        #region BASE
        protected virtual bool InitComponents(AppConfig config)
        {
            bool result = true;
            foreach (var item in components)
            {
                result &= item.Init(config);
            }
            return result;
        }

        protected virtual void Update()
        {
            float det = Time.deltaTime;
            foreach (var item in components)
            {
                item.GameUpdata(det);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            foreach (var item in components)
            {
                item.Exit();
            }
        }
        #endregion
    }
}
