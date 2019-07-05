using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startApp : MonoBehaviour
{

    private void Awake()
    {
        if(TestApp.inited == false)
        {
          TestApp.inited = true;
          var app =  new GameObject("App");
          app.AddComponent<TestApp>();
        }
    }

}
