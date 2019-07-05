using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

    private void Awake()
    {
        var prefab = Resources.Load<GameObject>("Canvas");
        GameObject.Instantiate<GameObject>(prefab);
    }

}
