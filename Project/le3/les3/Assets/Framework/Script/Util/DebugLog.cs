using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class DebugLog
    {
        static public void Log(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }

        static public void LogFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(format, args);
        }


        static public void LogWarning(string msg)
        {
            UnityEngine.Debug.LogWarning(msg);
        }

        static public void LogWarningFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(format, args);
        }

        static public void LogError(string msg)
        {
            UnityEngine.Debug.LogError(msg);
        }

        static public void LogErrorFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(format, args);
        }

    }

}

