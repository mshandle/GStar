using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeWithNameEx : MonoBehaviour
{
    [SerializeField]
    public string name = string.Empty;

    private void Awake()
    {
        Debug.LogFormat("Awake extension With:{0}", name);
    }
}
