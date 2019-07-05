using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class DebugLog
    {
        static public void Log(string msg)
        {
#if !RELESE
            UnityEngine.Debug.Log(msg);
#endif
        }

        static public void LogFormat(string format, params object[] args)
        {
#if !RELESE
            UnityEngine.Debug.LogFormat(format, args);
#endif
        }


        static public void LogWarning(string msg)
        {
            //Resources.Load<GameObject>("Cube.prefab");
#if !RELESE
            UnityEngine.Debug.LogWarning(msg);
#endif
        }

        static public void LogWarningFormat(string format, params object[] args)
        {
#if !RELESE
            UnityEngine.Debug.LogWarningFormat(format, args);
#endif
        }

        static public void LogError(string msg)
        {
#if !RELESE
            UnityEngine.Debug.LogError(msg);
#endif
        }

        static public void LogErrorFormat(string format, params object[] args)
        {
#if !RELESE
            UnityEngine.Debug.LogErrorFormat(format, args);
#endif
        }

    }

}

