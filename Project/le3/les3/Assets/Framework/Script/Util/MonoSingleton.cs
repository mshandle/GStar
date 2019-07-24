using UnityEngine;
namespace Framework
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T _instance;

        private static bool _bDestory = false;


        public static T Instance
        {
            get
            {
                if (_bDestory)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                    return null;
                }
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError("More than 1 !!");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        string name = typeof(T).Name;
                        Debug.Log("Instance Name : " + name);

                        GameObject instanceGO = GameObject.Find(name);
                        if (instanceGO == null)
                        {
                            instanceGO = new GameObject(name);
                        }
                        _instance = instanceGO.AddComponent<T>();

                        DontDestroyOnLoad(instanceGO);

                        Debug.Log("Add New Singleton " + _instance.name + " in Game");
                    }
                    else
                    {
                        Debug.LogWarning("Already Exist: " + _instance.name);
                    }
                }
                return _instance;
            }
        }

        public void ReleaseInstance()
        {
            _bDestory = true;
            _instance = null;
        }
    }
}