//-------------------------------------------------------------------------------------
//	SingletonPatternExample2.cs
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace SingletonPatternExample2
{
    public class SingletonPatternExample2 : MonoBehaviour
    {
        void Start()
        {
            RenderManager.Instance.Show();
            RenderManager.Instance.Show();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RenderManager.Instance.Show();
            }
        }
    }


    /// <summary>
    /// 某单例manager
    /// </summary>
    public class RenderManager
    {
        private static RenderManager instance = null;
        public static RenderManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RenderManager();
                }
                return instance;
            }
        }

        private RenderManager()
        {

        }
        /// <summary>
        /// 随便某方法
        /// </summary>
        public void Show()
        {
            Debug.Log("RenderManager is a Singleton !");
        }
    }

    public class Loger:Singleton<Loger>
    {
    
        public void DoSomeThings()
        {

        }
    }

    public class RenderManager2
    {
        private static RenderManager2 instance = new RenderManager2();
        public static RenderManager2 Instance
        {
            get { return instance; }
        }
    }





}