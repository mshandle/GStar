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

            bool result = true;
            foreach(var item in components)
            {
                result &= item.Init();
            }
            return result;
        }

        private T AddGameComponent<T>() where T : BaseComponentTemplate<T>
        {
            T component =  this.gameObject.AddComponent<T>();
            Type type = typeof(BaseComponentTemplate<>).MakeGenericType(typeof(T));
            var method =  type.GetMethod("RegisterComponent",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public);
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

        /*
        public T GetGameComponent<T>() where T : BaseComponentTemplate<T>
        {
            Type type = typeof(BaseComponentTemplate<>).MakeGenericType(typeof(T));
            string namekey = type.ToString();

            if (components.ContainsKey(namekey))
                return (T)components[namekey];
            return null;
        }
        */

    }
}
