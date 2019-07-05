using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class SceneMgr : BaseComponentTemplate<SceneMgr>
    {

        public override bool Init(AppConfig config)
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnUnityOnSceneLoad;
            DebugLog.Log("SceneMgr");
            return base.Init(config);
        }
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, BaseScene> scenes = new Dictionary<string, BaseScene>();

        /// <summary>
        /// 
        /// </summary>
        private BaseScene CurrentScene = null;

        #region Scene
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        public BaseScene RegisterSceneInfo(BaseScene scene)
        {
            if (scene == null) return null;

            if (!scenes.ContainsKey(scene.SceneName()))
            {
                scenes.Add(scene.SceneName(),scene);
            }
            else
            {
                DebugLog.LogWarningFormat("The same {0} already exists.", scene.SceneName());
            }
            return scene;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SceneName"></param>
        public void UnRegisterSceneInof(string SceneName)
        {
            if (scenes.ContainsKey(SceneName))
            {
                scenes.Remove(SceneName);
            }
        }

        public BaseScene GetSceneInfo(string SceneName)
        {
            BaseScene scene;
            if (scenes.TryGetValue(SceneName, out scene))
            {
                return scene;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SceneName"></param>
        public void LoadScene(string SceneName)
        {
            BaseScene scene = GetSceneInfo(SceneName);
            if(scene != null)
            {
                LoadScene(scene);
            }
            else
            {
                DebugLog.LogWarningFormat("Want Load Null Scene:{0}", SceneName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        public void LoadScene(BaseScene scene)
        {
            if (scene == null)
            {
                DebugLog.LogWarning("Want Load Null Scene");
                return;
            };
            var activeScene =  UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (!activeScene.name.Equals(scene.SceneName()))
            {
                var WillLoadScene =  UnityEngine.SceneManagement.SceneManager.GetSceneByName(scene.SceneName());
                UnityEngine.SceneManagement.SceneManager.LoadScene(scene.SceneName());
            }
            //
        }

        protected void OnUnityOnSceneLoad(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            var sceneInfo = GetSceneInfo(scene.name);
            OnLoadScene(sceneInfo);
        }

        protected void OnUnitySceneUnLoad(UnityEngine.SceneManagement.Scene sence)
        {
            //TODO:
        }
        protected void OnLoadScene(BaseScene scene)
        {
            if (scene == null) return;
            if(CurrentScene != null)
            {
                UnLoadScene(CurrentScene);
            }
            CurrentScene = scene;
            scene.OnLoadSceneCompleta();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SceneName"></param>
        public void UnLoadScene(string SceneName)
        {
            BaseScene scene = GetSceneInfo(SceneName);
            if (scene != null)
            {
                UnLoadScene(scene);
            }
            else
            {
                DebugLog.LogWarningFormat("Want Unload Null Scene:{0}", SceneName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        public void UnLoadScene(BaseScene scene)
        {
            scene.OnUnLoad();
        }

        #endregion
    }
}


